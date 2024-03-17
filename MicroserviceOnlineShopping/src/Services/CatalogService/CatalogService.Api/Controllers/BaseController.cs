using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        private IMediator? _mediator;
    }
}
