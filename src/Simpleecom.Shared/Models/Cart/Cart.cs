using Simpleecom.Shared.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Simpleecom.Shared.Models
{
    public class Cart 
    {
        public Cart()
        {
        }

        public Cart(CreateCartDto cart)
        {
            this.Products = cart.Products;  
            this.IsCompleted = cart.IsCompleted;    
            this.UserId = cart.UserId;      
        }

        public List<CartProduct>? Products { get; set;} = new List<CartProduct>();

        public bool IsCompleted { get; set; } = false;

        [Required]
        public string UserId { get; set; }
    }

    public class CartProduct
    {
        
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

    }

    public class CreateCartDto
    {
        public List<CartProduct> Products { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string UserId { get; set; }
    }

}
