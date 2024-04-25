using Microsoft.Azure.Cosmos;

namespace Simpleecom.Shared.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(string id, string partitionKeyValue);
        Task<IEnumerable<T>> GetByQueryAsync(QueryDefinition queryDefinition);
        Task<T> AddAsync(T item);
        Task UpdateAsync(string id, T item, string partitionKeyValue);
        Task DeleteAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(Func<T, bool> predicate);
    }
}
