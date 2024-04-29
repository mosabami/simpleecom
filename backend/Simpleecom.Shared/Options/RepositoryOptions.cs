namespace Simpleecom.Shared.Options
{
    public class RepositoryOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseId { get; set; }
        public string DatabaseName { get; set; }
        public string PartitionKey { get; set; }
        public string ContainerName { get; set; }
        public bool IsAutoResourceCreationIfNotExistsEnabled { get; set; }
        public bool AllowBulkExecution { get; set; }
        public bool ContainerPerItemType { get; set; }
        public string LeaseContainerName { get; internal set; }
    }

    public class ContainerOptions
    {
        public string ContainerId { get; set; }
        public string PartitionKeyPath { get; set; }
    }
}
