﻿using Newtonsoft.Json;
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

        public List<OrderProduct> Products { get; set; }

        public double OrderTotal { get; set; }

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

    }

    public class OrderProduct
    {
        
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
