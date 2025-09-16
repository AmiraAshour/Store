using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
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

    /// <summary>
    /// Add a product to the user's wishlist.
    /// </summary>
    [HttpPost("{productId}")]
    [SwaggerOperation(
        Summary = "Add to wishlist",
        Description = "Adds a product to the logged-in user's wishlist. " +
                      "If the product is already in the wishlist, it returns an error."
    )]
    [SwaggerResponse(200, "Product added to wishlist successfully")]
    [SwaggerResponse(400, "Invalid product ID or product already in wishlist")]
    [SwaggerResponse(401, "Unauthorized - user must be logged in")]
    public async Task<IActionResult> AddToWishlist(int productId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
      await _wishlistService.AddToWishlistAsync(userId, productId);
      return ApiResponseHelper.Success("", "Product added to wishlist");
    }

    /// <summary>
    /// Remove a product from the user's wishlist.
    /// </summary>
    [HttpDelete("{productId}")]
    [SwaggerOperation(
        Summary = "Remove from wishlist",
        Description = "Removes a product from the logged-in user's wishlist. " +
                      "If the product is not found in the wishlist, an error is returned."
    )]
    [SwaggerResponse(200, "Product removed from wishlist successfully")]
    [SwaggerResponse(404, "Product not found in wishlist")]
    [SwaggerResponse(401, "Unauthorized - user must be logged in")]
    public async Task<IActionResult> RemoveFromWishlist(int productId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
      await _wishlistService.RemoveFromWishlistAsync(userId, productId);
      return ApiResponseHelper.Success("", "Product removed from wishlist");
    }

    /// <summary>
    /// Get all products in the user's wishlist.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get wishlist",
        Description = "Retrieves all products currently saved in the logged-in user's wishlist."
    )]
    [SwaggerResponse(200, "Wishlist retrieved successfully")]
    [SwaggerResponse(401, "Unauthorized - user must be logged in")]
    public async Task<IActionResult> GetWishlist()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
      var wishlist = await _wishlistService.GetWishlistAsync(userId);
      return ApiResponseHelper.Success(wishlist, "Items retrieved successfully");
    }
  }
}
