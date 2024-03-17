using BasketService.Core.Consts;
using BasketService.Core.GeneralModels;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult Return(ApiReturnModel source)
        {
            switch (source.StatusCode)
            {
                case HttpStatusCodeEnum.BadRequest:
                case HttpStatusCodeEnum.InternalServerError:
                case HttpStatusCodeEnum.MethodNotAllowed:
                    return BadRequest(source);

                case HttpStatusCodeEnum.NotFound:
                    return NotFound(source);

                case HttpStatusCodeEnum.UnAuthorized:
                    return Unauthorized(source);

                default:
                    return Ok(source);
            }
        }
        protected IActionResult Return(Core.Results.IResult result)
            => result.Success ? Ok(result) :
                   result.Message == Messages.Unauthorized ? Unauthorized(new
                   {
                       Success = false,
                       Message = Messages.Unauthorized,
                   }) : BadRequest(result);
    }
}
