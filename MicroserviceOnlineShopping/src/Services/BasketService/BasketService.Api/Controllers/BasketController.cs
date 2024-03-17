using BasketService.Business.Services.Abstract;
using BasketService.Entity.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController(IBasketService basketService) : BaseController
    {
        private readonly IBasketService _basketService= basketService;
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasketByIdAsync(string id)=>
            Return(await _basketService.GetBasketByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> UpdateBasketAsync([FromBody] CustomerBasketRequestModel customerBasketRequestModel)=>
            Return(await _basketService.UpdateBasketAsync(customerBasketRequestModel));
      
        [HttpPut]
        public async Task<IActionResult> AddItemToBasketAsync([FromBody] BasketItemRequestModel basketItemRequestModel)=>
            Return(await _basketService.AddItemToBasketAsync(basketItemRequestModel));        

        [Route("Checkout")]
        [HttpPost]
        public async Task<IActionResult> CheckoutAsync([FromBody] BasketCheckoutRequestModel basketCheckoutRequestModel)=>
            Return(await _basketService.CheckoutAsync(basketCheckoutRequestModel));

        [HttpDelete("{id}")]     
        public async Task<IActionResult> DeleteBasketByIdAsync(string id)=>
            Return(await _basketService.DeleteBasketByIdAsync(id));
       
    }
}
