using BasketService.Business.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BasketService.Business.Services.Concrete
{
    public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor= httpContextAccessor;
        public string GetUserId()=>
            _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        
        public string GetUserEmail() =>
            _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Email).Value;
        
        public string GetUserName()=> 
            _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Name).Value;
    }
}
