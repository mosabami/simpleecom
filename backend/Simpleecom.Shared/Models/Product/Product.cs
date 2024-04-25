namespace Simpleecom.Shared.Models
{
    public class Product : Item
    {
        public Product(
            string name,
            string Description,
            string photoURL,
            double Price,
            int Inventory,
            string Brand,
            int productId
        )
        {
            this.Name = name;
            this.Description = Description;
            this.photoURL = photoURL;
            this.Price = Price;
            this.Inventory = Inventory;
            this.Brand = Brand;
            this.productId = productId;
        }

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string photoURL { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Inventory { get; set; }

        public string Brand { get; set; }

        public int productId { get; set; }

        public string GetPartitionKeyValue() => Brand;

    }
}
