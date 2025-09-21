using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Core.DTO.ProductDTO;
using Store.Core.Entities;
using Store.Core.Entities.comman;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.Core.Interfaces.ServiceInterfaces;

namespace Store.Core.Services
{
  public class WishlistService : IWishlistService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<WishlistService> _logger;

    public WishlistService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<WishlistService> logger)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task AddToWishlistAsync(string userId, int productId)
    {
      _logger.LogInformation("User {UserId} is attempting to add ProductId: {ProductId} to wishlist", userId, productId);

      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
      if (product == null)
      {
        _logger.LogWarning("User {UserId} tried to add non-existing ProductId: {ProductId} to wishlist", userId, productId);
        throw new ArgumentException("Product not found");
      }

      if (await _unitOfWork.wishlistRepository.ExistsAsync(userId, productId))
      {
        _logger.LogWarning("User {UserId} attempted to add duplicate ProductId: {ProductId} to wishlist", userId, productId);
        throw new InvalidOperationException("Product already in wishlist");
      }

      var item = new WishlistItem
      {
        UserId = userId,
        ProductId = productId
      };

      await _unitOfWork.wishlistRepository.AddAsync(item);
      _logger.LogInformation("ProductId: {ProductId} successfully added to wishlist by User {UserId}", productId, userId);
    }

    public async Task RemoveFromWishlistAsync(string userId, int productId)
    {
      _logger.LogInformation("User {UserId} is attempting to remove ProductId: {ProductId} from wishlist", userId, productId);

      if (!await _unitOfWork.wishlistRepository.ExistsAsync(userId, productId))
      {
        _logger.LogWarning("User {UserId} tried to remove non-existing ProductId: {ProductId} from wishlist", userId, productId);
        throw new KeyNotFoundException("Product not found in wishlist");
      }

      await _unitOfWork.wishlistRepository.RemoveAsync(userId, productId);
      _logger.LogInformation("ProductId: {ProductId} successfully removed from wishlist by User {UserId}", productId, userId);
    }

    public async Task<IReadOnlyList<ProductDTO>> GetWishlistAsync(string userId)
    {
      _logger.LogInformation("Fetching wishlist for User {UserId}", userId);

      var result = await _unitOfWork.wishlistRepository.GetByUserAsync(userId);

      var products = _mapper.Map<IReadOnlyList<ProductDTO>>(result.Select(x => x.Product));

      _logger.LogInformation("Retrieved {Count} products in wishlist for User {UserId}", products.Count, userId);

      return products;
    }
  }
}
