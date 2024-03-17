namespace BasketService.Entity.Models
{
    public class BasketCheckout
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpirationDate { get; set; }
        public string CardCvcCode { get; set; }
        public int CardTypeId { get; set; }
        public string Customer { get; set; }
    }
}
