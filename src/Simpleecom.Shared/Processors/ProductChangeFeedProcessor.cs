using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly RepositoryOptions _options;
        private static Random random = new Random();
        private readonly IServiceScopeFactory _scopeFactory;


        private readonly CosmosDBRepository<SimpleecomUser> _userRepository;
        private readonly CosmosDBRepository<Product> _productRepository;


        public ProductChangeFeedProcessor(IOptions<RepositoryOptions> options, 
            IServiceScopeFactory scopeFactory)
        {
            _options = options.Value;
            _scopeFactory = scopeFactory;
            _userRepository = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CosmosDBRepository<SimpleecomUser>>();
            _productRepository = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CosmosDBRepository<Product>>();

        }

        public async Task InitializeAsync()
        {
            string databaseName = _options.DatabaseId;
            string containerName = _options.ContainerName;

            var cosmosClient = new CosmosClientBuilder(_options.ConnectionString)
                .WithSerializerOptions(
                    new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                )
                .Build();

            //return new CosmosClient(configuration["CosmosDbEndpoint"], new DefaultAzureCredential());

            string leaseContainerName = $"Lease" + _options.ContainerName;
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            Container container = await database.CreateContainerIfNotExistsAsync(
                containerName,
                _options.PartitionKey
            );
            Container leaseContainer = await database.CreateContainerIfNotExistsAsync(
                leaseContainerName,
                "/id"
            );

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
                if(product.Active == false)
                {
                    var users = await _userRepository
                    .GetItemsAsync(x => x?.cart?.Products.Where(p => p.ProductId == product.Id)
                    .Count() > 1);
                  
                        foreach (var user in users)
                        {
                            user.cart.Products.Remove(user.cart.Products.FirstOrDefault(x => x.ProductId == product.Id));
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
    }
}
