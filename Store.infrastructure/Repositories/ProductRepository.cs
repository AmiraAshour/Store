using Store.Core.Entities.Product;
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
