

namespace Simpleecom.Shared.Models.Options
{
    public class CosmosConfiguration
    {
        public CosmosConfiguration(string accountEndPoint, 
            string accountKey, string databaseName, string containerName)
        {
            this.AccountEndpoint = accountEndPoint;
            this.AccountKey = accountKey;
            this.DatabaseName = databaseName;
            this.ContainerName = containerName;


        }
        public string AccountEndpoint { get; set; }
        public string AccountKey { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
    }
}
