

using Microsoft.EntityFrameworkCore;
using Store.Core.DTO.Product;
using Store.Core.Entities;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class WishlistRepository : IWishlistRepository
  {
    private readonly AppDbContext _context;

    public WishlistRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task AddAsync(WishlistItem item)
    {
      await _context.WishlistItems.AddAsync(item);
      await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string userId, int productId)
    {
      var item = await _context.WishlistItems
          .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

      if (item != null)
      {
        _context.WishlistItems.Remove(item);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<IReadOnlyList<WishlistItem>> GetByUserAsync(string userId)
    {
      return await _context.WishlistItems
        .Include(x => x.Product)
            .ThenInclude(p => p.Category)
        .Include(x => x.Product)
            .ThenInclude(p => p.Photos)
        .OrderByDescending(x => x.AddedAt)
        .ToListAsync();
    }

    public async Task<bool> ExistsAsync(string userId, int productId)
    {
      return await _context.WishlistItems
          .AnyAsync(x => x.UserId == userId && x.ProductId == productId);
    }

   
  }

}
