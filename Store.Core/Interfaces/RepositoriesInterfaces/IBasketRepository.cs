using Store.Core.Entities;

namespace Store.Core.Interfaces
{
  public interface IBasketRepository
  {
    Task<Basket?> GetBasketAsync(string id);
    Task<Basket?> UpdateBasketAsync(Basket basket);
    Task<bool> DeleteBasketAsync(string id);
  }
}
