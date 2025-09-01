

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO;
using Store.Core.Interfaces;
using System.Security.Claims;

namespace Store.API.Controllers
{
  [Authorize]
  public class ReviewsController : BaseController
  {
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
      _reviewService = reviewService;
    }

    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] AddReviewDTO dto)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var result = await _reviewService.AddReviewAsync(email!, dto);
      return ApiResponseHelper.Created(result, "Review added successfuly");
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductReviews(int productId)
    {
      var result = await _reviewService.GetProductReviewsAsync(productId);
      return ApiResponseHelper.Success(result, "Reviews retreve successfuly");
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var success = await _reviewService.DeleteReviewAsync(reviewId, email!);

      if (!success) return ApiResponseHelper.NotFound("The review not found");

      return ApiResponseHelper.Success("","Review deleted successfuly");
    }
    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] AddReviewDTO dto)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var updatedReview = await _reviewService.UpdateReviewAsync(reviewId, email!, dto);
      return ApiResponseHelper.Success(updatedReview, "Review updated successfuly");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("admin/{reviewId}")]
    public async Task<IActionResult> DeleteReviewByAdmin(int reviewId)
    {
      await _reviewService.DeleteReviewByAdminAsync(reviewId);
      return ApiResponseHelper.Success("","Review deleted by admin.");
    }

  }
}
