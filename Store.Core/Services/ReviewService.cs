using Microsoft.Extensions.Logging;
using Store.Core.DTO;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.Core.Services
{
  public class ReviewService : IReviewService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductsService _productService;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(IUnitOfWork unitOfWork, IProductsService productService, ILogger<ReviewService> logger)
    {
      _unitOfWork = unitOfWork;
      _productService = productService;
      _logger = logger;
    }

    public async Task<Review> AddReviewAsync(string bayerEmail, AddReviewDTO dto)
    {
      _logger.LogInformation("Attempting to add review for ProductId: {ProductId} by {Email}", dto.ProductId, bayerEmail);

      if (!await _unitOfWork.OrdersRepository.HasPurchased(bayerEmail, dto.ProductId))
      {
        _logger.LogWarning("User {Email} tried to review ProductId: {ProductId} without purchase", bayerEmail, dto.ProductId);
        throw new InvalidOperationException("You cannot review a product you haven't purchased.");
      }

      var existingReview = await _unitOfWork.ReviewRepository.GetByProductIdAsync(dto.ProductId);
      if (existingReview.Any(r => r.BuyerEmail == bayerEmail))
      {
        _logger.LogWarning("User {Email} attempted duplicate review for ProductId: {ProductId}", bayerEmail, dto.ProductId);
        throw new InvalidOperationException("You have already reviewed this product. Please update your review instead.");
      }

      var review = new Review
      {
        ProductId = dto.ProductId,
        BuyerEmail = bayerEmail,
        Rating = dto.Rating,
        Comment = dto.Comment
      };

      var added = await _unitOfWork.ReviewRepository.AddAsync(review);
      _logger.LogInformation("Review {ReviewId} added successfully for ProductId: {ProductId} by {Email}", added.Id, dto.ProductId, bayerEmail);

      await _productService.UpdateProductRatingAsync(dto.ProductId);
      _logger.LogInformation("Product rating updated for ProductId: {ProductId}", dto.ProductId);

      return added;
    }

    public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
    {
      _logger.LogInformation("Fetching reviews for ProductId: {ProductId}", productId);

      var reviews = await _unitOfWork.ReviewRepository.GetByProductIdAsync(productId);

      _logger.LogInformation("{Count} reviews retrieved for ProductId: {ProductId}", reviews.Count(), productId);

      return reviews;
    }

    public async Task<Review> UpdateReviewAsync(int reviewId, string bayerEmail, AddReviewDTO dto)
    {
      _logger.LogInformation("Attempting to update ReviewId: {ReviewId} by {Email}", reviewId, bayerEmail);

      var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
      if (existingReview == null)
      {
        _logger.LogWarning("ReviewId: {ReviewId} not found", reviewId);
        throw new KeyNotFoundException("Review not found.");
      }

      if (existingReview.BuyerEmail != bayerEmail)
      {
        _logger.LogWarning("Unauthorized update attempt for ReviewId: {ReviewId} by {Email}", reviewId, bayerEmail);
        throw new UnauthorizedAccessException("You can only update your own reviews.");
      }

      existingReview.Rating = dto.Rating;
      existingReview.Comment = dto.Comment;

      var updated = await _unitOfWork.ReviewRepository.UpdateAsync(existingReview);

      _logger.LogInformation("ReviewId: {ReviewId} updated successfully", reviewId);

      await _productService.UpdateProductRatingAsync(existingReview.ProductId);
      _logger.LogInformation("Product rating updated for ProductId: {ProductId}", existingReview.ProductId);

      return updated;
    }

    public async Task<bool> DeleteReviewAsync(int reviewId, string bayerEmail)
    {
      _logger.LogInformation("Attempting to delete ReviewId: {ReviewId} by {Email}", reviewId, bayerEmail);

      var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
      if (review == null)
      {
        _logger.LogWarning("ReviewId: {ReviewId} not found", reviewId);
        throw new KeyNotFoundException("Review not found.");
      }

      if (review.BuyerEmail != bayerEmail)
      {
        _logger.LogWarning("Unauthorized delete attempt for ReviewId: {ReviewId} by {Email}", reviewId, bayerEmail);
        throw new UnauthorizedAccessException("You can only delete your own reviews.");
      }

      var result = await _unitOfWork.ReviewRepository.DeleteAsync(reviewId, bayerEmail);

      _logger.LogInformation("ReviewId: {ReviewId} deleted successfully by {Email}", reviewId, bayerEmail);

      await _productService.UpdateProductRatingAsync(review.ProductId);
      _logger.LogInformation("Product rating updated for ProductId: {ProductId}", review.ProductId);

      return result;
    }

    public async Task DeleteReviewByAdminAsync(int reviewId)
    {
      _logger.LogInformation("Admin attempting to delete ReviewId: {ReviewId}", reviewId);

      var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
      if (review == null)
      {
        _logger.LogWarning("ReviewId: {ReviewId} not found for admin delete", reviewId);
        throw new KeyNotFoundException("Review not found.");
      }

      await _unitOfWork.ReviewRepository.DeleteAsync(reviewId, review.BuyerEmail);
      _logger.LogInformation("ReviewId: {ReviewId} deleted by admin", reviewId);

      await _productService.UpdateProductRatingAsync(review.ProductId);
      _logger.LogInformation("Product rating updated for ProductId: {ProductId} after admin deletion", review.ProductId);
    }
  }
}
