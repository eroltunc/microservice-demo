namespace OrderService.Application.Features.Queries.ViewModels
{
    public class OrderDetailViewModel
    {
        public string Ordernumber { get; init; }
        public string Status { get; init; }
        public string Description { get; init; }
        public string FullAddress { get; init; }
        public string City { get; init; }
        public string State { get; set; }
        public List<Orderitem> Orderitems { get; set; }
        public decimal Total { get; set; }
    }

    public class Orderitem
    {
        public string Productname { get; init; }
        public int Units { get; init; }
        public double Unitprice { get; init; }
        public string Pictureurl { get; init; }
    }
}
