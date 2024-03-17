using AutoMapper;
using BasketService.Application.IntegrationEvents.Events;
using BasketService.Business.Services.Abstract;
using BasketService.Core.Results;
using BasketService.DataAccess.Abstract;
using BasketService.Entity.Models;
using BasketService.Entity.Models.RequestModels;
using BasketService.Entity.Models.ResponseModels;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;

namespace BasketService.Business.Services.Concrete
{
    public class BasketService
        (
            IBasketDal basketDal,
            IIdentityService identityService,
            IEventBus eventBus,
            ILogger<BasketService> logger,
            IMapper mapper
        ) : IBasketService
    {
        private readonly IBasketDal _basketDal = basketDal;
        private readonly IIdentityService _identityService = identityService;
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger<BasketService> _logger = logger;
        private readonly IMapper _mapper = mapper;


        public async Task<IResult> AddItemToBasketAsync(BasketItemRequestModel basketItemRequestModel)
        {
            var basket = await _basketDal.GetBasketAsync(GetUserId());
            if (basket == null)
                basket = new CustomerBasket(GetUserId(), GetUserName(), GetUserEmail());          

            basket.Items.Add(_mapper.Map<BasketItem>(basketItemRequestModel));
            await _basketDal.UpdateBasketAsync(basket);
            return new SuccessResult();
        }
      

        public async Task<IResult> CheckoutAsync(BasketCheckoutRequestModel basketCheckoutRequestModelRequestModel)
        {
            var basket = await _basketDal.GetBasketAsync(GetUserId());

            if (basket is null)
                return new ErrorResult();
            
            var eventMessage = new OrderCreatedIntegrationEvent(
                GetUserId(), 
                GetUserName(), GetUserName(),
                basketCheckoutRequestModelRequestModel.City, 
                basketCheckoutRequestModelRequestModel.FullAddress,
                basketCheckoutRequestModelRequestModel.State, 
                basketCheckoutRequestModelRequestModel.CardNumber,
                basketCheckoutRequestModelRequestModel.CardHolderName,
                basketCheckoutRequestModelRequestModel.CardExpiration, 
                basketCheckoutRequestModelRequestModel.CardCvcCode,
                basketCheckoutRequestModelRequestModel.CardTypeId,                
                basket);

            try
            {
                _eventBus.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                throw;
            }

            return new SuccessResult();
        }

        public async Task<IResult> DeleteBasketByIdAsync(string id)
        {
            await _basketDal.DeleteBasketAsync(id);
            return new SuccessResult();
        }

        public async Task<IDataResult<CustomerBasketResponseModel>> GetBasketByIdAsync(string id)
        {
            var basket = await _basketDal.GetBasketAsync(id);
            if (basket == null)
            {
                return new ErrorDataResult<CustomerBasketResponseModel>(new CustomerBasketResponseModel(id));
            }
            return new SuccessDataResult<CustomerBasketResponseModel>(_mapper.Map<CustomerBasketResponseModel>(basket));

        }

        public async Task<IDataResult<CustomerBasketResponseModel>> UpdateBasketAsync(CustomerBasketRequestModel customerBasketRequestModel)
        {
            var responseModel = await _basketDal.UpdateBasketAsync(_mapper.Map<CustomerBasket>(customerBasketRequestModel));
            return new SuccessDataResult<CustomerBasketResponseModel>(_mapper.Map<CustomerBasketResponseModel>(responseModel));

        }
        private string GetUserId() => _identityService.GetUserId().ToString();
        private string GetUserEmail() => _identityService.GetUserEmail().ToString();
        private string GetUserName() => _identityService.GetUserName().ToString();
    }
}
