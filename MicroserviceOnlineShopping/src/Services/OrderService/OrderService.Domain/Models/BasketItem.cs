﻿namespace OrderService.Domain.Models
{
    public class BasketItem
    {
        public string Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ProductPictureUrl { get; set; }
    }
}
