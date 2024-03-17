using OrderService.Domain.Exceptions;
using OrderService.Domain.SeedWork;

namespace OrderService.Domain.AggregateModels.CustomerAggregate
{
    public class PaymentMethod : BaseEntity
    {
        public string CardNumber { get; set; }
        public string CvcCode { get; set; }
        public string CardholderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }      
        public int CardTypeId { get; set; }
        public CardType CardType { get; private set; }
        public PaymentMethod() { }
        public PaymentMethod(int cardTypeId, string description, string cardNumber, string cvcCode, string cardHolderName, DateTime expirationDate)
        {
            CardNumber = cardNumber;
            CvcCode = cvcCode;
            CardholderName = cardHolderName;
            Description = description;
            ExpirationDate = expirationDate;
            CardTypeId = cardTypeId;
        }
        public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration) =>       
            (CardTypeId == cardTypeId && CardNumber == cardNumber && ExpirationDate == expiration);
        
    }
}
