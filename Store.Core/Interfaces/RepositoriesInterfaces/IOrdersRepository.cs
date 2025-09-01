using Store.Core.Entities.Order;

namespace Store.Core.Interfaces.RepositoriesInterfaces
{
  public interface IOrdersRepository
  {
    Task<Orders> AddOrederAsync(Orders oreder);
    Task<IReadOnlyList<Orders>> GetAllOrdersForUserAsync(string BuyerEmail);
    Task<Orders?> GetOrderByIdAsync(int Id);
    Task <Orders?> UpdateOrderAsync(Orders oreder);
    Task<bool> HasPurchased(string buyerEmail, int productId);
  }
}
