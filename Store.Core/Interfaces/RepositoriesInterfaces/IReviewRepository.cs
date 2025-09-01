

using Store.Core.Entities;

namespace Store.Core.Interfaces.RepositoriesInterfaces
{
  public interface IReviewRepository
  {
    Task<Review> AddAsync(Review review);
    Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
    Task<bool> DeleteAsync(int reviewId, string bayerEmail);
    Task<Review?> GetByIdAsync(int reviewId);
    Task<Review> UpdateAsync(Review review);
  }
}
