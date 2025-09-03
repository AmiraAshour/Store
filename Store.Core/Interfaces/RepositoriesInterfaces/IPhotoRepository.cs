
using Store.Core.Entities.ProductEntity;

namespace Store.Core.Interfaces.RepositoriesInterFaces
{
  public interface IPhotoRepository:IGenericReposeitry<Photo>
  {
    Task<bool> AddRangeAsync(IEnumerable<Photo> photos);
    Task<bool> DeleteRangeAsync(IEnumerable<Photo> photos);
    Task<ICollection<Photo>?> GetPhotosByProductIdAsync(int productId);
    }
}
