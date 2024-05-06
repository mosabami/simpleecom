using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Options;

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

        public ProductChangeFeedProcessor(IOptions<RepositoryOptions> options)
        {
            _options = options.Value;
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
