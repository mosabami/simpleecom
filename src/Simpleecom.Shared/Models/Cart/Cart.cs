using Simpleecom.Shared.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Simpleecom.Shared.Models
{
    public class Cart : Item
    {
        public Cart(List<CartProduct> products, bool isCompleted, string userId)
        {
            this.Products = products;
            this.IsCompleted = isCompleted;
            this.UserId = userId;
        }

        public List<CartProduct> Products { get; set;}

        public bool IsCompleted { get; set; } = false;

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
    }

    public class CartProduct
    {
        
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

    }
}
