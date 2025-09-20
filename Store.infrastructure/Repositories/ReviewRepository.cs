using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.comman;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.infrastructure.Data;


namespace Store.infrastructure.Repositories
{
  public class ReviewRepository : IReviewRepository
  {
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<Review> AddAsync(Review review)
    {
      _context.Reviews.Add(review);
      await _context.SaveChangesAsync();
      return review;
    }

    public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
    {
      return await _context.Reviews
          .Where(r => r.ProductId == productId)
          .OrderByDescending(r => r.CreatedAt)
          .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int reviewId, string bayerEmail)
    {
      var review = await _context.Reviews
          .FirstOrDefaultAsync(r => r.Id == reviewId && r.BuyerEmail == bayerEmail);

      if (review == null) return false;

      _context.Reviews.Remove(review);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<Review?> GetByIdAsync(int reviewId)
    {
      var review= await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
      return review;
    }

    public async Task<Review> UpdateAsync(Review review)
    {
      _context.Reviews.Update(review);
      await _context.SaveChangesAsync();
      return review;
    }
  }
}
