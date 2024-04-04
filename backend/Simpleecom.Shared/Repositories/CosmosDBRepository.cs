using Microsoft.Azure.Cosmos;
using Simpleecom.Shared.Models;

namespace Simpleecom.Shared.Repositories
{

    public class CosmosDBRepository<T> : IRepository<T> where T : Item
    {
        private readonly Container _container;

        public CosmosDBRepository(Container container)
        {
            _container = container;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetByQueryAsync( QueryDefinition queryDefinition)
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

        public async Task UpdateAsync(string id,T item, string partitionKeyValue = null)
        {
            await _container.ReplaceItemAsync(item, item.Id, new PartitionKey(partitionKeyValue ?? id));
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
                throw ex;
            }
        }

    }

}