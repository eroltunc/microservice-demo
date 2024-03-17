using AutoMapper;
using BasketService.Entity.Models;
using BasketService.Entity.Models.RequestModels;
using BasketService.Entity.Models.ResponseModels;
namespace BasketService.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<BasketCheckout, BasketItemResponseModel>();
            CreateMap<BasketItem, BasketItemResponseModel>();
            CreateMap<CustomerBasket, CustomerBasketResponseModel>();
            CreateMap<CustomerBasketRequestModel, CustomerBasket>();
            CreateMap<BasketCheckoutRequestModel, BasketCheckout>();
            CreateMap<BasketItemRequestModel, BasketItem>();
        }
    }
}
