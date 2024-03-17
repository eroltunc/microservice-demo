using OrderService.Domain.Events;
using OrderService.Domain.SeedWork;

namespace OrderService.Domain.AggregateModels.CustomerAggregate
{
    public class Customer : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }

        private List<PaymentMethod> _paymentMethods;
        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        protected Customer()
        {
            _paymentMethods = new List<PaymentMethod>();
        }

        public Customer(string name):this()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }


        public PaymentMethod VerifyOrAddPaymentMethod(int cardTypeId, string description, string cardNumber,string cvcCode, string cardHolderName, DateTime expiration, Guid orderId)
        {
            var existingPayment = _paymentMethods.SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

            if (existingPayment != null)
            {
                AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));
                return existingPayment;
            }

            var payment = new PaymentMethod(cardTypeId, description, cardNumber, cvcCode, cardHolderName, expiration);
            _paymentMethods.Add(payment);

            AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));
            return payment;
        }




        public override bool Equals(object obj)
        {
            return base.Equals(obj) ||
                   (obj is Customer buyer &&
                   Id.Equals(buyer.Id) &&
                   Name == buyer.Name);
        }
    }
}
