using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Interfaces;
using System.Text.Json;

namespace Store.infrastructure.Repositories
{
  public class BasketRepository : IBasketRepository
  {
    private readonly IDatabase _database;
    public BasketRepository(IConnectionMultiplexer redis)
    {
      _database = redis.GetDatabase();
    }



    public async Task<Basket?> GetBasketAsync(string id)
    {
      var result = await _database.StringGetAsync(id);
      if (string.IsNullOrEmpty(result))
      {
        return null;
      }
      return JsonSerializer.Deserialize<Basket>(result);
    }

    public async Task<Basket?> UpdateBasketAsync(Basket basket)
    {
      var result =await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));
      if (result)
      {
        return await GetBasketAsync(basket.Id)??basket;
      }
      return null;

    }

    public async Task<bool> DeleteBasketAsync(string id)
    {
      var result = await _database.KeyDeleteAsync(id);
      return result;
    }
  }
}
