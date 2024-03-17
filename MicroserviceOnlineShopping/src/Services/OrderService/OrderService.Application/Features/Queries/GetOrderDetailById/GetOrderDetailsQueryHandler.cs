using AutoMapper;
using MediatR;
using OrderService.Application.Features.Queries.ViewModels;
using OrderService.Application.Interfaces.Repositories;

namespace OrderService.Application.Features.Queries.GetOrderDetailById
{
    public class GetOrderDetailsQueryHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<GetOrderDetailsQuery, OrderDetailViewModel>
    {
        IOrderRepository orderRepository= orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        private readonly IMapper mapper = mapper;
        public async Task<OrderDetailViewModel> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, i => i.OrderItems);

            var result = mapper.Map<OrderDetailViewModel>(order);

            return result;
        }
    }
}
