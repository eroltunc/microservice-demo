using BasketService.Entity.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Entity.Models.RequestModels
{
    public class CustomerBasketRequestModel
    {
        public string BuyerId { get; set; }
        public CustomerBasketRequestModel(){}
        public List<BasketItemRequestModel> Items { get; set; } = new List<BasketItemRequestModel>();
        public CustomerBasketRequestModel(string customerId)
        {
            BuyerId = customerId;
        }
    }
}
