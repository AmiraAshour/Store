

using Store.Core.DTO.ProductDTO;
using Store.Core.Entities.ProductEntity;

namespace Store.Core.Interfaces.RepositoriesInterFaces
{
  public interface IProductRepository:IGenericReposeitry<Product>
  {
    IQueryable<ProductSalesDto> GetTopSellingProductsAsync(int? categoryId = null);
    // for future specific methods related to products, if needed
  }
}
