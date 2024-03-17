namespace OrderService.Domain.AggregateModels.OrderAggregate
{
    public record Address
    {
        public String FullAddress { get; private set; }
        public String City { get; private set; }
        public String State { get; private set; }
        public Address(){}
        public Address(string city, string state, string fullAddress )
        {
            FullAddress = fullAddress;
            City = city;
            State = state;
        }
    }
}
