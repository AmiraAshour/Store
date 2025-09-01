using Store.Core.DTO;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.Core.Services
{
  public class ReviewService : IReviewService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductsService _productService;

    public ReviewService(IUnitOfWork unitOfWork, IProductsService productService)
    {
      _unitOfWork = unitOfWork;
      _productService = productService;
    }

    public async Task<Review> AddReviewAsync(string bayerEmail, AddReviewDTO dto)
    {
      // Check if user bought the product
      if (!await _unitOfWork.OrdersRepository.HasPurchased(bayerEmail, dto.ProductId))
      {
        throw new InvalidOperationException("You cannot review a product you haven't purchased.");
      }

      // Check if user already reviewed this product
      var existingReview = await _unitOfWork.ReviewRepository.GetByProductIdAsync(dto.ProductId);

      if (existingReview.Any(r => r.BuyerEmail == bayerEmail))
        throw new InvalidOperationException("You have already reviewed this product. Please update your review instead.");

      // Add the review
      var review = new Review
      {
        ProductId = dto.ProductId,
        BuyerEmail = bayerEmail,
        Rating = dto.Rating,
        Comment = dto.Comment
      };
      var added = await _unitOfWork.ReviewRepository.AddAsync(review);
      // Update product's average rating
      await _productService.UpdateProductRatingAsync(dto.ProductId);
      return added;
    }

    public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
    {
      var reviews = await _unitOfWork.ReviewRepository.GetByProductIdAsync(productId);
      return reviews;
    }

    public async Task<Review> UpdateReviewAsync(int reviewId, string bayerEmail, AddReviewDTO dto)
    {
      var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
      if (existingReview == null)
      {
        throw new KeyNotFoundException("Review not found.");
      }

      if (existingReview.BuyerEmail != bayerEmail)
      {
        throw new UnauthorizedAccessException("You can only update your own reviews.");
      }

      existingReview.Rating = dto.Rating;
      existingReview.Comment = dto.Comment;

      var updated = await _unitOfWork.ReviewRepository.UpdateAsync(existingReview);

      await _productService.UpdateProductRatingAsync(existingReview.ProductId);
      return updated;
    }

    public async Task<bool> DeleteReviewAsync(int reviewId, string bayerEmail)
    {

      var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
      if (review == null)
        throw new KeyNotFoundException("Review not found.");

      if (review.BuyerEmail != bayerEmail)
        throw new UnauthorizedAccessException("You can only delete your own reviews.");

      var result= await _unitOfWork.ReviewRepository.DeleteAsync(reviewId, bayerEmail);

      await _productService.UpdateProductRatingAsync(review.ProductId);
      return result;
    }
    public async Task DeleteReviewByAdminAsync(int reviewId)
    {
      var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
      if (review == null) throw new KeyNotFoundException();

      await  _unitOfWork.ReviewRepository.DeleteAsync(reviewId, review.BuyerEmail);

      await _productService. UpdateProductRatingAsync(review.ProductId);
    }


  }
}
