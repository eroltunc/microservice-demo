using AutoMapper;
using CatalogService.Application.Models.Dto.Catalog;
using CatalogService.Domain.AggregateModels.CatalogAggregate;

namespace CatalogService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogItem, CatalogItemDto>();
        }
    }
}
