using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.ProductEntity;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class PhotoRepository : GenericReposeitory<Photo>, IPhotoRepository
  {
    private readonly AppDbContext _context;
    public PhotoRepository(AppDbContext context) : base(context)
    {
      _context = context;
    }

    public async Task<bool> AddRangeAsync(IEnumerable<Photo> photos)
    {
      await  _context.Photos.AddRangeAsync(photos);
     var result= await  _context.SaveChangesAsync();
      return result > 0;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<Photo> photos)
    {
      if (photos == null || !photos.Any())
      {
        return false;
      }
       _context.Photos.RemoveRange(photos);
      var result =await _context.SaveChangesAsync();
      return result > 0;
    }

    public async Task<ICollection<Photo>?> GetPhotosByProductIdAsync(int productId)
    {
      if (productId <= 0)
      {
        throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));
      }
      var photos =await _context.Photos.Where(p => p.ProductId == productId).ToListAsync();
      return photos;
    }
  }
}
