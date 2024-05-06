﻿namespace Simpleecom.Shared.Models
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
            string productId
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

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string PhotoURL { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Inventory { get; set; }

        public string Brand { get; set; }

        private string _productId;
        public string ProductId
        {
            get
            {

                return _productId;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _productId = Guid.NewGuid().ToString();
                }
                else
                {
                    _productId = value;
                }
            }
        }

        public string GetPartitionKeyValue() => Brand;

    }
}