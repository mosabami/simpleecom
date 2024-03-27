using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpleecom.Shared.Options
{
    public class RepositoryOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseId { get; set; }
        public bool IsAutoResourceCreationIfNotExistsEnabled { get; set; }
        public bool AllowBulkExecution { get; set; }
        public bool ContainerPerItemType { get; set; }

    }

    public class ContainerOptions 
    {
        public string ContainerId { get; set; }
        public string PartitionKeyPath { get; set; }
    }
}
