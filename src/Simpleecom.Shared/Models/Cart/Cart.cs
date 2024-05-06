using Simpleecom.Shared.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Simpleecom.Shared.Models
{
    public class Cart : Item
    {
        public Cart(List<CartProduct> products, string status, string userId)
        {
            this.Products = products;
            this.Status = status;
            this.UserId = userId;
        }

        public List<CartProduct> Products { get; set;}

        public string Status { get; set; }

        [Required]
        public string UserId { get; set; }

        private string _cartId;
        public string CartId
        {
            get
            {

                return _cartId;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _cartId = Guid.NewGuid().ToString();
                }
                else
                {
                    _cartId = value;
                }
            }
        }

        protected override string GetPartitionKeyValue() => UserId;
    }

    public class CartProduct
    {
        
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

    }
}
