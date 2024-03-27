using Microsoft.Azure.Cosmos;
using Microsoft.Azure.CosmosRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpleecom.Shared.Models
{
    public class Order : Item
    {

        public Order(double total, string status, string UserId,List<Product> products, bool isUpdate = false)
        {
            this.Total = total;
            this.Status = status;
            this.UserId = UserId;
            this.Products = products;
            this.IsUpdate = isUpdate;
            PartitionKey = UserId.ToString();
            
        }

        public Order(double total, string status, string UserId, List<Product> products, string partitionKey ,bool isUpdate = false)
        {
            this.Total = total;
            this.Status = status;
            this.UserId = UserId;
            this.Products = products;
            this.IsUpdate = isUpdate;
            PartitionKey = partitionKey;
        }

        public List<Product> Products { get; set; }

        public int Quantity { get; set; }

        public double Total { get; set; }

        public string Status { get; set; }

        public string UserId { get; set;}

        public bool IsUpdate { get; set; }

        public string PartitionKey { get; set; }

        public string OrderId { get; set; }

        protected override string GetPartitionKeyValue() => PartitionKey;
    }

}
