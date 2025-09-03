

using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.Interfaces;
using System.Security.Claims;

namespace Store.API.Controllers
{
    public class WishlistController : BaseController
  {
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
      _wishlistService = wishlistService;
    }

    [HttpPost("{productId}")]
    public async Task<IActionResult> AddToWishlist(int productId)
    {

      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
      await _wishlistService.AddToWishlistAsync(userId, productId);
      return ApiResponseHelper.Success("", "Product added to wishlist");
    }


    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveFromWishlist(int productId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
      await _wishlistService.RemoveFromWishlistAsync(userId, productId);
      return ApiResponseHelper.Success("","Product removed from wishlist");
    }

    [HttpGet]
    public async Task<IActionResult> GetWishlist()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
      var wishlist = await _wishlistService.GetWishlistAsync(userId);
      return ApiResponseHelper.Success(wishlist,"Items retreve successfuly");
    }
  }
}
