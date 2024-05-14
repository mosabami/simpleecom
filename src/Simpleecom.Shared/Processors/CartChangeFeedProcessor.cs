using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Options;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Shared.Processors
{
    public interface ICartChangeFeedProcessor
    {
        Task InitializeAsync();
    }

    public class CartChangeFeedProcessor : ICartChangeFeedProcessor
    {
        private readonly CosmosDbOptions _options;
        private static Random random = new Random();
        private readonly CosmosDBRepository<Order> _repository;
        private readonly IServiceScopeFactory _scopeFactory;
        private CosmosClient _cosmosClient;


        public CartChangeFeedProcessor(IOptions<CosmosDbOptions> options, IServiceScopeFactory scopeFactory)
        {
            _options = options.Value;
            _scopeFactory = scopeFactory;
            _repository = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CosmosDBRepository<Order>>();
        }

        public async Task InitializeAsync()
        {
            string databaseName = _options.DATABASE_ID;
            string containerName = _options.CONTAINER_NAME;



            _cosmosClient = GetCosmosClient();

            //return new CosmosClient(configuration["CosmosDbEndpoint"], new DefaultAzureCredential());

            string leaseContainerName = $"Lease" + _options.CONTAINER_NAME;
            Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            Container container = await database.CreateContainerIfNotExistsAsync(
                containerName,
                _options.PARTITION_KEY
            );
            Container leaseContainer = await database.CreateContainerIfNotExistsAsync(
                leaseContainerName,
                "/id"
            );

            ChangeFeedProcessor changeFeedProcessor = container
                .GetChangeFeedProcessorBuilder<Cart>("changeFeedProcessor", HandleChangesAsync)
                .WithInstanceName($"LeaseInstance-{random}")
                .WithLeaseContainer(leaseContainer)
                .Build();

            await changeFeedProcessor.StartAsync();
        }

        private async Task HandleChangesAsync(
            IReadOnlyCollection<Cart> changes,
            CancellationToken cancellationToken
        )
        {
            foreach (var cart in changes)
            { 
                Console.WriteLine($"Changed item: {cart}");
            }
            return;
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
