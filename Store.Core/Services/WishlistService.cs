

using AutoMapper;
using Store.Core.DTO.Product;
using Store.Core.Entities;
using Store.Core.Interfaces;
using Store.Core.Interfaces.RepositoriesInterfaces;

namespace Store.Core.Services
{
  public class WishlistService : IWishlistService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WishlistService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }


    public async Task AddToWishlistAsync(string userId, int productId)
    {

      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
      if (product == null)
        throw new ArgumentException("Product not found");

      if (await _unitOfWork.wishlistRepository.ExistsAsync(userId, productId))
        throw new InvalidOperationException("Product already in wishlist");

      var item = new WishlistItem
      {
        UserId = userId,
        ProductId = productId
      };

      await _unitOfWork.wishlistRepository.AddAsync(item);
    }

    public async Task RemoveFromWishlistAsync(string userId, int productId)
    {
      if (!await _unitOfWork.wishlistRepository.ExistsAsync(userId, productId))
        throw new KeyNotFoundException("Product not found in wishlist");

      await _unitOfWork.wishlistRepository.RemoveAsync(userId, productId);
    }

    public async Task<IReadOnlyList<ProductDTO>> GetWishlistAsync(string userId)
    {
      var result= await _unitOfWork.wishlistRepository.GetByUserAsync(userId);
      var products=_mapper.Map<IReadOnlyList<ProductDTO>>(result.Select(x=>x.Product));
      return products;
    }
  }

}
