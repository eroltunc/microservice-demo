using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.CustomerAggregate;
using OrderService.Infrastructure.Context;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(OrderDbContext dbContext) : base(dbContext) { }
   
    }
}
