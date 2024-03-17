using IdentityService.Business.Services;
using IdentityService.Entity.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequestModel userRequestModel) =>
            Return(await _authService.Register(userRequestModel));
       
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel loginUserRequestModel)=>
             Return(await _authService.Login(loginUserRequestModel, null, null));
            
        [HttpGet("info"),Authorize]
        public async Task<IActionResult> Info() => 
            Return(await _authService.Info());
       
    }
}
