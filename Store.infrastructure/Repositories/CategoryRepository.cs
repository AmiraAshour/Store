
using Store.Core.Entities.ProductEntity;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class CategoryRepository : GenericReposeitory<Category>, ICategoryRepository
  {
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
  }
}
