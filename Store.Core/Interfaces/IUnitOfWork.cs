using Store.Core.Interfaces.RepositoriesInterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces
{
  public interface IUnitOfWork
  {
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IPhotoRepository PhotoRepository { get; }
    public IBasketRepository BasketRepository { get; }
  }
}
