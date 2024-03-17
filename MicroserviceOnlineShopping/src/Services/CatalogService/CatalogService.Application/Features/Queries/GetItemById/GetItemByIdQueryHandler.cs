using AutoMapper;
using CatalogService.Application.Features.Queries.GetAllItem;
using CatalogService.Application.Models.Dto.Catalog;
using CatalogService.Application.Models.PaginatedModels;
using CatalogService.Application.Repositories;
using MediatR;

namespace CatalogService.Application.Features.Queries.GetItemById
{
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQueryRequest, CatalogItemDto>
    {
        private readonly ICatalogRepository _catalogReadRepository;
        private readonly IMapper _mapper;
        public GetItemByIdQueryHandler(ICatalogRepository catalogReadRepository, IMapper mapper)
        {
            _catalogReadRepository = catalogReadRepository;
            _mapper = mapper;
        }
        public async Task<CatalogItemDto> Handle(GetItemByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var parseBool = (Ok: Guid.TryParse(request.Id, out Guid x), Value: x);
            if (parseBool.Ok)
            {
                var item = await _catalogReadRepository.GetByIdAsync(parseBool.Value);
                return _mapper.Map<CatalogItemDto>(item);
            }
            return new CatalogItemDto();


            
        }
    }
}
