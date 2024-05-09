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
        private readonly RepositoryOptions _options;
        private static Random random = new Random();
        private readonly CosmosDBRepository<Order> _repository;
        private readonly IServiceScopeFactory _scopeFactory;


        public CartChangeFeedProcessor(IOptions<RepositoryOptions> options, IServiceScopeFactory scopeFactory)
        {
            _options = options.Value;
            _scopeFactory = scopeFactory;
            _repository = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CosmosDBRepository<Order>>();
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
    }
}
