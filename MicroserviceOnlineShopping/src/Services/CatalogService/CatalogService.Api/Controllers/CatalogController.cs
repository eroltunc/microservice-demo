using CatalogService.Application.Features.Queries.GetAllItem;
using CatalogService.Application.Features.Queries.GetItemById;
using CatalogService.Application.Models.Dto.Catalog;
using CatalogService.Application.Models.PaginatedModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class CatalogController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("items")]      
        public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            PaginatedItemsViewModel<CatalogItemDto> response = await Mediator.Send(new GetAllItemQueryRequest() { Ids = ids, PageSize = pageSize, PageIndex = pageIndex });
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("items/{id}")]
        public async Task<IActionResult> ItemByIdAsync(string id)
        {
            CatalogItemDto response = await Mediator.Send(new GetItemByIdQueryRequest() { Id = id });
            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        [Route("update-item-price")]
        public async Task<IActionResult> UpdateItemPrice()
        {
            return Ok();
        }
        }
}
