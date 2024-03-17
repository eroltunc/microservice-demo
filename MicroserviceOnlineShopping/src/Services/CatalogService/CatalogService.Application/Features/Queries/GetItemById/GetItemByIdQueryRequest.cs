using CatalogService.Application.Models.Dto.Catalog;
using CatalogService.Application.Models.PaginatedModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Features.Queries.GetItemById
{
    public class GetItemByIdQueryRequest : IRequest<CatalogItemDto>
    {
        public string Id { get; set; }
    }
}
