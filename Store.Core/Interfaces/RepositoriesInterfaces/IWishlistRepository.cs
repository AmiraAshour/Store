using Store.Core.Entities.comman;

namespace Store.Core.Interfaces.RepositoriesInterfaces
{
  public interface IWishlistRepository
  {
    Task AddAsync(WishlistItem item);
    Task RemoveAsync(string userId, int productId);
    Task<IReadOnlyList<WishlistItem>> GetByUserAsync(string userId);
    Task<bool> ExistsAsync(string userId, int productId);
  }

}
