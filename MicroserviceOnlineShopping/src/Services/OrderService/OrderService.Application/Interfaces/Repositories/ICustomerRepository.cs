using OrderService.Domain.AggregateModels.CustomerAggregate;

namespace OrderService.Application.Interfaces.Repositories
{
    public interface ICustomerRepository: IGenericRepository<Customer>
    {
    }
}
