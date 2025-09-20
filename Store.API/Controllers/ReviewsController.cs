using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO;
using Store.Core.Interfaces.ServiceInterfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Store.API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class ReviewsController : BaseController
  {
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
      _reviewService = reviewService;
    }

    /// <summary>
    /// Add a review for a purchased product.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Add a new review",
        Description = "Allows a user to add a review for a product they have purchased. " +
                      "Users can only review a product once."
    )]
    [SwaggerResponse(201, "Review added successfully")]
    [SwaggerResponse(400, "Invalid request or user has already reviewed this product")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> AddReview([FromBody] AddReviewDTO dto)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var result = await _reviewService.AddReviewAsync(email!, dto);
      return ApiResponseHelper.Created(result, "Review added successfuly");
    }

    /// <summary>
    /// Get all reviews for a specific product.
    /// </summary>
    [HttpGet("{productId}")]
    [SwaggerOperation(
        Summary = "Get product reviews",
        Description = "Retrieves all reviews for a given product."
    )]
    [SwaggerResponse(200, "Reviews retrieved successfully")]
    [SwaggerResponse(404, "Product not found or no reviews exist")]
    public async Task<IActionResult> GetProductReviews(int productId)
    {
      var result = await _reviewService.GetProductReviewsAsync(productId);
      return ApiResponseHelper.Success(result, "Reviews retrieved successfully");
    }

    /// <summary>
    /// Delete a review created by the logged-in user.
    /// </summary>
    [HttpDelete("{reviewId}")]
    [SwaggerOperation(
        Summary = "Delete own review",
        Description = "Allows a user to delete their own review for a product."
    )]
    [SwaggerResponse(200, "Review deleted successfully")]
    [SwaggerResponse(404, "Review not found")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var success = await _reviewService.DeleteReviewAsync(reviewId, email!);

      if (!success) return ApiResponseHelper.NotFound("The review not found");

      return ApiResponseHelper.Success("", "Review deleted successfully");
    }

    /// <summary>
    /// Update an existing review created by the logged-in user.
    /// </summary>
    [HttpPut("{reviewId}")]
    [SwaggerOperation(
        Summary = "Update own review",
        Description = "Allows a user to update their own review by review ID."
    )]
    [SwaggerResponse(200, "Review updated successfully")]
    [SwaggerResponse(404, "Review not found")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] AddReviewDTO dto)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var updatedReview = await _reviewService.UpdateReviewAsync(reviewId, email!, dto);
      return ApiResponseHelper.Success(updatedReview, "Review updated successfully");
    }

    /// <summary>
    /// Delete any review (Admin only).
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("admin/{reviewId}")]
    [SwaggerOperation(
        Summary = "Delete review by Admin",
        Description = "Allows an Admin to delete any review by review ID."
    )]
    [SwaggerResponse(200, "Review deleted by Admin successfully")]
    [SwaggerResponse(404, "Review not found")]
    [SwaggerResponse(403, "Forbidden - Only Admins can access this endpoint")]
    public async Task<IActionResult> DeleteReviewByAdmin(int reviewId)
    {
      await _reviewService.DeleteReviewByAdminAsync(reviewId);
      return ApiResponseHelper.Success("", "Review deleted by admin.");
    }
  }
}
