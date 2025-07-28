using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Product;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
