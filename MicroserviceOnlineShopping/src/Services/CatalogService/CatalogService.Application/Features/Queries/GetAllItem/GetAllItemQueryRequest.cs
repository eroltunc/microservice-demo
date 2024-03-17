using CatalogService.Application.Models.Dto.Catalog;
using CatalogService.Application.Models.PaginatedModels;
using CatalogService.Domain.AggregateModels.CatalogAggregate;
using MediatR;

namespace CatalogService.Application.Features.Queries.GetAllItem
{
    public class GetAllItemQueryRequest:IRequest<PaginatedItemsViewModel<CatalogItemDto>>
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public string Ids { get; set; } = null;
    }
}
