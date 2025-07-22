using Store.Core.Entities.Product;
using Store.Core.Interfaces;
using Store.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.infrastructure.Repositories
{
  public class CategoryRepository : GenericReposeitory<Category>, ICategoryRepository
  {
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
  }
}
