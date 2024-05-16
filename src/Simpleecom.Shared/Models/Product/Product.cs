using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Simpleecom.Shared.Models
{
    public class Product : Item
    {
        public Product()
        {
        }

        public Product(CreateProductDto product)
        {
            this.Name = product.Name;
            this.Description = product.Description;
            this.PhotoURL = product.PhotoURL;
            this.Price = product.Price;
            this.Inventory = product.Inventory;
            this.Brand = product.Brand;
            this.Id = product.Id;
        }

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string PhotoURL { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Inventory { get; set; }

        [JsonIgnore]
        public bool Active { get; set; } = true;

        [Required]
        public string Brand { get; set; }

        public string GetPartitionKeyValue() => Brand;

    }

    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public string PhotoURL { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public int Inventory { get; set; }

        [Required]
        public string Brand { get; set; }

    }
}
