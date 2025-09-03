using Store.Core.Entities.ProductEntity;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class ProductRepository : GenericReposeitory<Product>, IProductRepository
  {
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
  }
}
