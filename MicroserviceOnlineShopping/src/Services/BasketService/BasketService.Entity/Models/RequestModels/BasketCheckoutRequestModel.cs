using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Entity.Models.RequestModels
{
    public class BasketCheckoutRequestModel
    {
        //Address
        public string City { get; set; }        
        public string State { get; set; }
        public string FullAddress { get; set; }
        //Payment
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public string CardCvcCode { get; set; }
        public int CardTypeId { get; set; }
        //User
        public string CustomerId { get; set; }
    }
}
