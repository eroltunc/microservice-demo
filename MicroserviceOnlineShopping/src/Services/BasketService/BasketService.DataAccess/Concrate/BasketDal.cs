using BasketService.DataAccess.Abstract;
using BasketService.Entity.Models;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace BasketService.DataAccess.Concrate
{
    public class BasketDal(ILoggerFactory loggerFactory, ConnectionMultiplexer redis) : IBasketDal
    {
        private readonly ConnectionMultiplexer _redis = redis;
        private readonly IDatabase _database = redis.GetDatabase();
        public async Task<bool> DeleteBasketAsync(string id)=>
            await _database.KeyDeleteAsync(id);
        

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data = await _database.StringGetAsync(customerId);

            if (data.IsNullOrEmpty)
                return null;
            return JsonConvert.DeserializeObject<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.CustomerId, JsonConvert.SerializeObject(basket));

            if (!created)
                return null;
            return await GetBasketAsync(basket.CustomerId);
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
