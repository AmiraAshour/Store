using Store.Core.DTO;
using Store.Core.Entities.comman;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IReviewService
  {
    Task<Review> AddReviewAsync(string bayerEmail, AddReviewDTO dto);
    Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
    Task<Review> UpdateReviewAsync(int reviewId, string bayerEmail, AddReviewDTO dto);
    Task<bool> DeleteReviewAsync(int reviewId, string bayerEmail);
    Task DeleteReviewByAdminAsync(int reviewId);
  }
}
