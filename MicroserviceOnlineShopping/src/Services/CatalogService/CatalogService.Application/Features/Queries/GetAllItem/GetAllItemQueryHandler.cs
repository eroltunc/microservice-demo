using AutoMapper;
using CatalogService.Application.Models.Dto.Catalog;
using CatalogService.Application.Models.PaginatedModels;
using CatalogService.Application.Repositories;
using CatalogService.Domain.AggregateModels.CatalogAggregate;
using MediatR;

namespace CatalogService.Application.Features.Queries.GetAllItem
{
    public class GetAllItemQueryHandler : IRequestHandler<GetAllItemQueryRequest, PaginatedItemsViewModel<CatalogItemDto>>
    {
        private readonly ICatalogRepository _catalogReadRepository;
        private readonly IMapper _mapper;
        public GetAllItemQueryHandler(ICatalogRepository catalogReadRepository, IMapper mapper)
        {
            _catalogReadRepository = catalogReadRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsViewModel<CatalogItemDto>> Handle(GetAllItemQueryRequest request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Ids))
            {
                var items = await GetItemsByIdsAsync(request.Ids);

                if (!items.Any())
                {
                    return new PaginatedItemsViewModel<CatalogItemDto>(request.PageIndex, request.PageSize, 0, new List<CatalogItemDto>());
                }

                return new PaginatedItemsViewModel<CatalogItemDto>(
                    request.PageIndex, 
                    request.PageSize,
                    items.Count(), 
                    _mapper.Map<List<CatalogItemDto>>(
                        items.OrderBy(c => c.Name).Skip(request.PageSize * request.PageIndex).Take(request.PageSize).ToList()));
            }

            var allItems = await _catalogReadRepository.GetAll();
            allItems = ChangeUriPlaceholder(allItems);
            var totalItems= allItems.LongCount(); 
            return new PaginatedItemsViewModel<CatalogItemDto>(
                request.PageIndex,
                request.PageSize,
                totalItems,
                _mapper.Map<List<CatalogItemDto>>(
                    allItems.OrderBy(c => c.Name).Skip(request.PageSize * request.PageIndex).Take(request.PageSize).ToList()));
            
        }
        private async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: Guid.TryParse(id, out Guid x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<CatalogItem>();
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = await _catalogReadRepository.GetAll();

            items = items.Where(ci => idsToSelect.Contains(ci.Id)).ToList();

            items = ChangeUriPlaceholder(items);

            return items;
        }
        private List<CatalogItem> ChangeUriPlaceholder(List<CatalogItem> items)
        {
            var baseUri = "Pics/";
            foreach (var item in items)
                if (item != null)
                    item.PictureUri = baseUri + item.PictureFileName;
            return items;
        }
    }
}
