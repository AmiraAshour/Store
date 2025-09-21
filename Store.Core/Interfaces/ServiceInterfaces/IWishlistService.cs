using Store.Core.DTO.ProductDTO;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IWishlistService
  {
    Task AddToWishlistAsync(string userId, int productId);
    Task RemoveFromWishlistAsync(string userId, int productId);
    Task<IReadOnlyList<ProductDTO>> GetWishlistAsync(string userId);
  }

}
