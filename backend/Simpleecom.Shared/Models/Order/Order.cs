namespace Simpleecom.Shared.Models
{
    public class Order : Item
    {
       

        public Order(
            double total,
            string status,
            string UserId,
            List<Product> products,
            string OrderId,
            bool isUpdate = false
        )
        {
            this.Total = total;
            this.Status = status;
            this.UserId = UserId;
            this.Products = products;
            this.IsUpdate = isUpdate;
            this.OrderId = OrderId;
        }

        public List<Product> Products { get; set; }

        public int Quantity { get; set; }

        public double Total { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public bool IsUpdate { get; set; }

        public string OrderId { get; set; }

        protected override string GetPartitionKeyValue() => UserId;
    }
}
