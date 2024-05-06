using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Options;

namespace Simpleecom.Shared.Repositories
{
    public class CosmosDBRepository<T> : IRepository<T>
        where T : Item
    {
        private readonly Container _container;
        private readonly RepositoryOptions _options;

        public CosmosDBRepository(IOptions<RepositoryOptions> options)
        {
            _options = options.Value;


            var cosmosClient = new CosmosClientBuilder(_options.ConnectionString)
                .WithSerializerOptions(
                    new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                )
                .Build();

            _container = cosmosClient
                .GetDatabase(_options.DatabaseId)
                .CreateContainerIfNotExistsAsync(_options.ContainerName, _options.PartitionKey)
                .Result;
        }

        public async Task<T> GetByIdAsync(string id, string partitionKeyValue)
        {
            try
            {
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
            ItemResponse<T> response = await _container.CreateItemAsync(item);
            return response.Resource;
        }

        public async Task UpdateAsync(string id, T item, string partitionKeyValue)
        {
            await _container.ReplaceItemAsync(
                item,
                item.Id,
                new PartitionKey(partitionKeyValue)
            );
        }
        public async Task<T> UpsertAsync(T item)
        {
            ItemResponse<T> response = await _container.UpsertItemAsync(item);
            return response.Resource;
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Func<T, bool> predicate)
        {
            try
            {
                var items = _container.GetItemLinqQueryable<T>(true).Where(predicate);
                return items; // or throw an exception if not found
            }
            catch (Exception ex)
            {
               throw new Exception($"Item not found " + ex.Message);
            }
        }
    }
}
