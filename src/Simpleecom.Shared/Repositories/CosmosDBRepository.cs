using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Options;
using SimpleecomUser = Simpleecom.Shared.Models.User.User;

namespace Simpleecom.Shared.Repositories
{
    public class CosmosDBRepository<T> : IRepository<T>
        where T : Item
    {
        private Container _container;
        private readonly CosmosDbOptions _options;
        private CosmosClient _cosmosClient;

        public CosmosDBRepository(IOptions<CosmosDbOptions> options)
        {
            _options = options.Value;
            _cosmosClient = GetCosmosClient<T>();
        }

        private CosmosClient GetCosmosClient<T>()
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

        private Container GetCosmosContainer()
        {
            var containerName = GetContainerName<T>();
            var container = _cosmosClient.GetContainer(_options.DATABASE_ID, containerName);
            return container;
        }

        public async Task<T> GetByIdAsync(string id, string partitionKeyValue)
        {
            try
            {
                _container = GetCosmosContainer();
                ItemResponse<T> response = await _container.ReadItemAsync<T>(
                    id,
                    new PartitionKey(partitionKeyValue)
                );
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception($"Item with id {id} not found");
            }
        }

        public async Task<IEnumerable<T>> GetByQueryAsync(QueryDefinition queryDefinition)
        {
            _container = GetCosmosContainer();
            var query = _container.GetItemQueryIterator<T>(queryDefinition);
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<T> AddAsync(T item)
        {
            _container = GetCosmosContainer();
            ItemResponse<T> response = await _container.CreateItemAsync(item);
            return response.Resource;
        }

        public async Task<T> UpsertAsync(T item)
        {
            _container = GetCosmosContainer();
            ItemResponse<T> response = await _container.UpsertItemAsync(item);
            return response.Resource;
        }

        public async Task DeleteAsync(string id, string partitionKeyValue)
        {
            _container = GetCosmosContainer();
            await _container.DeleteItemAsync<T>(id, new PartitionKey(partitionKeyValue));
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Func<T, bool> predicate)
        {
            try
            {
                _container = GetCosmosContainer();
                var items = _container.GetItemLinqQueryable<T>(true).Where(predicate);
                return items; // or throw an exception if not found
            }
            catch (Exception ex)
            {
                throw new Exception($"Item not found " + ex.Message);
            }
        }

        public async Task<T> GetItemAsync(Func<T, bool> predicate)
        {
            try
            {
                _container = GetCosmosContainer();
                var item = _container.GetItemLinqQueryable<T>(true).Where(predicate)
                    .AsEnumerable()
                    .FirstOrDefault();
                return item; // or throw an exception if not found
            }
            catch (Exception ex)
            {
                throw new Exception($"Item not found " + ex.Message);
            }
        }

        public Task UpdateAsync(string id, T item, string partitionKeyValue)
        {
            throw new NotImplementedException();
        }

        private static string GetContainerName<T>()
        {
            Type t = typeof(T);

            switch (t)
            {
                case Type _ when t == typeof(Order):
                    return "Orders";
                case Type _ when t == typeof(Product):
                    return "Products";
                case Type _ when t == typeof(SimpleecomUser):
                    return "Users";
                case Type _ when t == typeof(Cart):
                    return "Carts";
                default:
                    return null;
            }
        }
    }
}
