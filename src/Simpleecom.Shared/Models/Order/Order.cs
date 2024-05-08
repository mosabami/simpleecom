using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Simpleecom.Shared.Models
{
    public class Order : Item
    {

        public Order()
        {
        }

        public Order(
            string UserId,
            List<OrderProduct> products,
            string OrderId,
            bool isCompleted = false
        )
        {
            this.UserId = UserId;
            this.Products = products;
            this.IsCompleted = isCompleted;
            this.OrderId = OrderId;
        }

        public Order(CreateOrderDto order)
        {
            this.UserId = order.UserId;
            this.Products = order.Products;
            this.OrderTotal = order.OrderTotal;
        }

        public Order(Cart cart)
        {
            this.UserId = cart.UserId;
            this.Products = GetOrderProducts(cart.Products);
        }

        public List<OrderProduct> Products { get; set; }

        private double _orderTotal;
        public double OrderTotal
        {
            get
            {
                double total = 0;
                foreach (var product in Products)
                {
                    total += product.ProductPrice * product.ProductQuantity;
                }
                return total;
            }
            set
            {
                _orderTotal = value;
            }
        }

        [Required]
        public string UserId { get; set; }

        public bool IsCompleted { get; set; } = false;

        private string _orderId;
        public string OrderId
        {
            get
            {

                return base.Id;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _orderId = Guid.NewGuid().ToString();
                }
                else
                {
                    _orderId = value;
                }
            }
        }

        private List<OrderProduct> GetOrderProducts(List<CartProduct> cartProducts)
        {
            var items = new List<OrderProduct>();
            foreach (var cartProduct in cartProducts)
            {
                items.Add(new OrderProduct(cartProduct));
            }
            return items;
        }

    }

    public class OrderProduct
    {
        public OrderProduct()
        {
        }

        public OrderProduct(CartProduct cartProduct)
        {
            this.ProductId = cartProduct.ProductId;
            this.ProductName = cartProduct.ProductName;
            this.ProductPrice = cartProduct.ProductPrice;
            this.ProductQuantity = cartProduct.ProductQuantity;
        }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

    }

    public class CreateOrderDto
    {
        public List<OrderProduct> Products { get; set; }
        public string UserId { get; set; }
        public double OrderTotal { get; set; }

    }

}
