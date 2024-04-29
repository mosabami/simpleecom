using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;
using Simpleecom.Shared.Constants;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Options;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Shared.Processors
{
    public interface IOrderChangeFeedProcessor
    {
        Task InitializeAsync();
    }

    public class OrderChangeFeedProcessor : IOrderChangeFeedProcessor
    {
        private readonly RepositoryOptions _options;
        private static Random random = new Random();
        private readonly CosmosDBRepository<Product> _repository;


        public OrderChangeFeedProcessor(IOptions<RepositoryOptions> options, CosmosDBRepository<Product> repository)
        {
            _options = options.Value;
            _repository = repository;
        }

        public async Task InitializeAsync()
        {
            string databaseName = _options.DatabaseName;
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

            string leaseContainerName = _options.LeaseContainerName;
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
                .GetChangeFeedProcessorBuilder<Order>("changeFeedProcessor", HandleChangesAsync)
                .WithInstanceName($"LeaseInstance-{random}")
                .WithLeaseContainer(leaseContainer)
                .Build();

            await changeFeedProcessor.StartAsync();
        }

        private async Task HandleChangesAsync(
            IReadOnlyCollection<Order> changes,
            CancellationToken cancellationToken
        )
        {
            foreach (var order in changes)
            {
                if (order.Status == OrderStatus.Completed)
                {
                    foreach (var item in order.Products)
                    {
                        var product = await _repository.GetByIdAsync(item.Id, _options.PartitionKey);
                        product.Inventory -= item.Inventory;
                        await _repository.UpdateAsync(product.Id, product, product.Brand);
                    }
                    Console.WriteLine($"Order {order.Id} is complete");
                }
          
                Console.WriteLine($"Changed item: {order}");
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
