using Store.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces.RepositoriesInterFaces
{
  public interface IPhotoRepository:IGenericReposeitry<Photo>
  {
    Task<bool> AddRangeAsync(IEnumerable<Photo> photos);
    Task<bool> DeleteRangeAsync(IEnumerable<Photo> photos);
    Task<ICollection<Photo>?> GetPhotosByProductIdAsync(int productId);
    }
}
