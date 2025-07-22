using Store.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces
{
  public interface IProductRepository:IGenericReposeitry<Product>
  {
    // for future specific methods related to products, if needed
  }
}
