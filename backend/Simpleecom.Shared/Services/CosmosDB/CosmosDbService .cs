using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Simpleecom.Shared.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly string _endpointUrl;
        private readonly string _primaryKey;

        public CosmosDbService(IConfiguration configuration)
        {
            _endpointUrl = configuration["CosmosDb:EndpointUrl"];
            _primaryKey = configuration["CosmosDb:PrimaryKey"];
        }

        public CosmosClient GetClient()
        {
            return new CosmosClient(_endpointUrl, _primaryKey);
        }
    }

}
