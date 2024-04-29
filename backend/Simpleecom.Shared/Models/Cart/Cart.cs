namespace Simpleecom.Shared.Models
{
    public class Cart : Item
    {
        public Cart(List<Product> products, int quantity, string status, string userId)
        {
            this.Products = products;
            this.Quantity = quantity;
            this.Status = status;
            this.UserId = userId;
            PartitionKey = userId;
        }

        public Cart(
            List<Product> products,
            int quantity,
            string status,
            string userId,
            string partitionKey
        )
        {
            this.Products = products;
            this.Quantity = quantity;
            this.Status = status;
            this.UserId = userId;
            PartitionKey = partitionKey;
        }

        public List<Product> Products { get; set; }

        public int Quantity { get; set; }

        public double Total
        {
            get
            {
                double total = 0;
                foreach (Product product in Products)
                {
                    total += product.Price;
                }
                return total;
            }
        }

        public string Status { get; set; }

        public string UserId { get; set; }

        public string CartId { get; set; }

        public string PartitionKey { get; set; }

        protected override string GetPartitionKeyValue() => PartitionKey;
    }
}
