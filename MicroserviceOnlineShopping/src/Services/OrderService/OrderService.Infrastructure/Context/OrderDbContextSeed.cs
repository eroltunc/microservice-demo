using Microsoft.Data.SqlClient;
using OrderService.Domain.AggregateModels.CustomerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using Polly;

namespace OrderService.Persistence.Context
{
    public class OrderDbContextSeed
    {
        //private static readonly List<OrderStatus> orderStatuses=new List<OrderStatus>()
        //{
        //    new OrderStatus(1,"Submitted"),
        //    new OrderStatus(2,"AwaitingValidation"),
        //    new OrderStatus(3,"StockConfirmed"),
        //    new OrderStatus(4,"Paid"),
        //    new OrderStatus(5,"Shipped"),
        //    new OrderStatus(6,"SubmiCancelledtted"),
        //};
        //private static readonly List<CardType> cardTypes = new List<CardType>()
        //{
        //    new CardType(1,"Visa"),
        //    new CardType(2,"MasterCard")
        //};
        public async Task SeedAsync(OrderDbContext context)
        {
            var policy = Policy.Handle<SqlException>().
                 WaitAndRetryAsync(
                     retryCount: 3,
                     sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                     onRetry: (exception, timeSpan, retry, ctx) =>
                     {
                     }
                 );

            await policy.ExecuteAsync(() => ProcessSeeding(context));
        }
        private async Task ProcessSeeding(OrderDbContext context)
        {
            if (!context.OrderStatus.Any())
            {
                await context.OrderStatus.AddRangeAsync(GetOrderStatus());
                await context.SaveChangesAsync();
            }
            if (!context.CardTypes.Any())
            {
                await context.CardTypes.AddRangeAsync(GetCardTypes());
                await context.SaveChangesAsync();
            }           
        }
        private IEnumerable<OrderStatus> GetOrderStatus()
        {
            return new List<OrderStatus>()
            {
                OrderStatus.Submitted,
                OrderStatus.AwaitingValidation,
                OrderStatus.StockConfirmed,
                OrderStatus.Paid,
                OrderStatus.Shipped,
                OrderStatus.Cancelled
            };
        }
        private IEnumerable<CardType> GetCardTypes()
        {
            return new List<CardType>()
            {
                 CardType.Visa,
                CardType.MasterCard             
             
            };
        }
    }
}
