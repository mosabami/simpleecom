using Newtonsoft.Json;

namespace Simpleecom.Shared.Models
{
    public class Order : Item
    {
       

        public Order(
            string UserId,
            List<OrderProduct> products,
            string OrderId,
            bool isUpdate = false
        )
        {
            this.UserId = UserId;
            this.Products = products;
            this.IsUpdate = isUpdate;
            this.OrderId = OrderId;
        }


        public List<OrderProduct> Products { get; set; }

        public double OrderTotal { get; set; }

        public string UserId { get; set; }

        public bool IsUpdate { get; set; }

        private string _orderId;
        public string OrderId
        {
            get
            {
                
               return _orderId;
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

    }

    public class OrderProduct
    {

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

    }
}
