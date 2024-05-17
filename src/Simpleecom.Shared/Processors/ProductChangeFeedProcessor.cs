using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Options;
using Simpleecom.Shared.Repositories;
using SimpleecomUser = Simpleecom.Shared.Models.User.User;

namespace Simpleecom.Shared.Processors
{
    public interface IProductChangeFeedProcessor
    {
        Task InitializeAsync();
    }

    public class ProductChangeFeedProcessor : IProductChangeFeedProcessor
    {
        private readonly CosmosDbOptions _options;
        private static Random random = new Random();
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ProductChangeFeedProcessor> _logger;


        private readonly CosmosDBRepository<SimpleecomUser> _userRepository;
        private readonly CosmosDBRepository<Product> _productRepository;
        private CosmosClient _cosmosClient;



        public ProductChangeFeedProcessor(IOptions<CosmosDbOptions> options,
            IServiceScopeFactory scopeFactory)
        {
            _options = options.Value;
            _scopeFactory = scopeFactory;
            _userRepository = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CosmosDBRepository<SimpleecomUser>>();
            _productRepository = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CosmosDBRepository<Product>>();
            _logger = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ILogger<ProductChangeFeedProcessor>>();

        }

        public async Task InitializeAsync()
        {
            string databaseName = _options.DATABASE_ID;
            string containerName = _options.CONTAINER_NAME;

            _cosmosClient = GetCosmosClient();

            string leaseContainerName = $"Lease" + _options.CONTAINER_NAME;
            _logger.LogInformation("Creating database");
            Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            _logger.LogInformation("Creating container");
            Container container = await database.CreateContainerIfNotExistsAsync(
                containerName,
                _options.PARTITION_KEY
            );
            _logger.LogInformation("Creating lease container");
            Container leaseContainer = await database.CreateContainerIfNotExistsAsync(
                leaseContainerName,
                "/id"
            );
            _logger.LogInformation("Done creating containers");

            ChangeFeedProcessor changeFeedProcessor = container
                .GetChangeFeedProcessorBuilder<Product>("changeFeedProcessor", HandleChangesAsync)
                .WithInstanceName($"LeaseInstance-{random}")
                .WithLeaseContainer(leaseContainer)
                .Build();

            await changeFeedProcessor.StartAsync();
        }

        private async Task HandleChangesAsync(
            IReadOnlyCollection<Product> changes,
            CancellationToken cancellationToken
        )
        {
            foreach (var product in changes)
            {
                if (product.Active == false)
                {
                    var users = await _userRepository.GetItemsAsync(x => x?.cart?.Products
                    .Where(cp => cp.ProductId == product.Id).Count() >= 1);

                    foreach (var user in users)
                    {
                        user?.cart?.Products?.Remove(user.cart.Products.FirstOrDefault(x => x.ProductId == product.Id));
                        await _userRepository.UpsertAsync(user);
                    }
                    var pk = product.GetPartitionKeyValue();
                    await _productRepository.DeleteAsync(product.Id, pk);
                }
                Console.WriteLine($"Changed item: {product}");
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }

        private CosmosClient GetCosmosClient()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                _cosmosClient = new CosmosClientBuilder(_options.CONNECTION_STRING)
                           .WithSerializerOptions(new CosmosSerializationOptions
                           {
                               PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                           }).Build();
            }
            else
            {
                _cosmosClient = new CosmosClientBuilder(_options.COSMOS_ENDPOINT, new DefaultAzureCredential())
                           .WithSerializerOptions(new CosmosSerializationOptions
                           {
                               PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                           }).Build();
            }

            return _cosmosClient;
        }
    }
}
