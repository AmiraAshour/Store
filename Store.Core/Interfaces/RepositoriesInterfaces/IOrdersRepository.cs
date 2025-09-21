using Store.Core.Entities.Order;
using System.Linq.Expressions;

namespace Store.Core.Interfaces.RepositoriesInterfaces
{
  public interface IOrdersRepository
  {
    Task<Orders> AddOrederAsync(Orders oreder);
    Task<IReadOnlyList<Orders>> GetAllOrdersForUserAsync(string BuyerEmail);
    Task<Orders?> GetOrderByIdAsync(int Id);
    Task <Orders?> UpdateOrderAsync(Orders oreder);
    Task<bool> HasPurchased(string buyerEmail, int productId);
    Task<IEnumerable<Orders>> GetAllOrdersAsync(Expression<Func<Orders, bool>> filter);
    Task<IEnumerable<Orders>?> GetAllOrderUndeliverdAsync();
  }
}
