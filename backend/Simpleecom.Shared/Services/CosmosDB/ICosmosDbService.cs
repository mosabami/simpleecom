using Microsoft.Azure.Cosmos;

namespace Simpleecom.Shared.Services
{
    public interface ICosmosDbService
    {
        CosmosClient GetClient();
    }
}