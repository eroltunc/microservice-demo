namespace BasketService.Entity.Models.ResponseModels
{
    public class CustomerBasketResponseModel
    {
        public string BuyerId { get; set; }
        public CustomerBasketResponseModel()
        {

        }
        public List<BasketItemResponseModel> Items { get; set; } = new List<BasketItemResponseModel>();
        public CustomerBasketResponseModel(string customerId)
        {
            BuyerId = customerId;
        }
    }
}
