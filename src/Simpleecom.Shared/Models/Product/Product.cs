using System.ComponentModel.DataAnnotations;

namespace Simpleecom.Shared.Models
{
    public class Product : Item
    {
        public Product()
        {
        }

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
            this.PhotoURL = photoURL;
            this.Price = Price;
            this.Inventory = Inventory;
            this.Brand = Brand;
            this.ProductId = productId;
        }

        public Product(CreateProductDto product)
        {
            this.Name = product.Name;
            this.Description = product.Description;
            this.PhotoURL = product.PhotoURL;
            this.Price = product.Price;
            this.Inventory = product.Inventory;
            this.Brand = product.Brand;
            this.ProductId = product.ProductId;
        }

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string PhotoURL { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Inventory { get; set; }

        [Required]
        public string Brand { get; set; }

        public int ProductId { get; set; }

        public string GetPartitionKeyValue() => Brand;

    }

    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string PhotoURL { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public int Inventory { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
