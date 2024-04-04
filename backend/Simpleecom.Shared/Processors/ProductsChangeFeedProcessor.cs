using System;
using System.Configuration;
using System.Threading.Tasks;
using Azure.Identity;
using CosmosDBChangeFeedSample;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CosmosDBChangeFeedSample
{
    public static class CosmosDBChangeFeedExtensions
    {
        public static IServiceCollection AddCosmosDBChangeFeed(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.Configure<CosmosDBOptions>(configuration.GetSection("CosmosDB"));
            services.AddSingleton<ICosmosDBChangeFeedInitializer, CosmosDBChangeFeedInitializer>();
            return services;
        }
    }

    public interface ICosmosDBChangeFeedInitializer
    {
        Task InitializeAsync();
    }

    public class CosmosDBChangeFeedInitializer : ICosmosDBChangeFeedInitializer
    {
        private readonly CosmosDBOptions _options;

        public CosmosDBChangeFeedInitializer(IOptions<CosmosDBOptions> options)
        {
            _options = options.Value;
        }

        public async Task InitializeAsync()
        {
            string endpointUri = _options.EndpointUri;
            string primaryKey = _options.PrimaryKey;
            string databaseName = _options.DatabaseName;
            string containerName = _options.ContainerName;

            CosmosClient cosmosClient = new CosmosClient(endpointUri, primaryKey);
            CosmosClient cosmosClient2 = new CosmosClient("connectionstring");
            //return new CosmosClient(configuration["CosmosDbEndpoint"], new DefaultAzureCredential());

            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            Container container = await database.CreateContainerIfNotExistsAsync(containerName, "/partitionKey");

            ChangeFeedProcessor changeFeedProcessor = container
                .GetChangeFeedProcessorBuilder<dynamic>("changeFeedProcessor", HandleChangesAsync)
                .WithInstanceName("consoleHost")
                .WithLeaseContainer(container)
                .Build();

            await changeFeedProcessor.StartAsync();
        }

        private async Task HandleChangesAsync(IReadOnlyCollection<dynamic> changes, CancellationToken cancellationToken)
        {
            foreach (var change in changes)
            {
                Console.WriteLine($"Changed item: {change}");
            }
        }
    }

    public class CosmosDBOptions
    {
        public string EndpointUri { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
    }
}


//public void ConfigureServices(IServiceCollection services)
//{
//    services.AddCosmosDBChangeFeed(Configuration);
//    // other service configurations
//}


//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//namespace CosmosDBChangeFeedSample
//{
//    class Program
//    {
//        static async Task Main(string[] args)
//        {
//            var host = Host.CreateDefaultBuilder(args)
//                .ConfigureServices((context, services) =>
//                {
//                    services.AddCosmosDBChangeFeed(context.Configuration);
//                })
//                .Build();

//            var initializer = host.Services.GetRequiredService<ICosmosDBChangeFeedInitializer>();
//            await initializer.InitializeAsync();

//            await host.RunAsync();
//        }
//    }
//}
