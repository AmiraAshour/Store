using Store.Core.Entities.BasketEntity;

namespace Store.Core.Interfaces
{
  public interface IBasketRepository
  {
    Task<Basket?> GetBasketAsync(string id);
    Task<Basket?> UpdateBasketAsync(Basket basket);
    Task<bool> DeleteBasketAsync(string id);
  }
}
