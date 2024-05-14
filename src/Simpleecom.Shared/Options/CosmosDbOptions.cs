namespace Simpleecom.Shared.Options
{
    public class CosmosDbOptions
    {
        public string CONNECTION_STRING { get; set; }
        public string DATABASE_ID { get; set; }
        public string PARTITION_KEY { get; set; }
        public string CONTAINER_NAME { get; set; }
        public string COSMOS_ENDPOINT { get; set; }
    }
}
