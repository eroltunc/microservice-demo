using BasketService.Core.Results;
using BasketService.DataAccess.Abstract;
using BasketService.Entity.Models.RequestModels;
using BasketService.Entity.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Business.Services.Abstract
{
  
    public interface IBasketService
    {
        Task<IDataResult<CustomerBasketResponseModel>> GetBasketByIdAsync(string id);
        Task<IDataResult<CustomerBasketResponseModel>> UpdateBasketAsync(CustomerBasketRequestModel customerBasketRequestModel);
        Task<IResult> AddItemToBasketAsync(BasketItemRequestModel basketItemRequestModel);
        Task<IResult> CheckoutAsync(BasketCheckoutRequestModel basketCheckoutRequestModel);
        Task<IResult> DeleteBasketByIdAsync(string id);
        

    }
}
