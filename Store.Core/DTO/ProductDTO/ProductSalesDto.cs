

using Store.Core.Entities.ProductEntity;

namespace Store.Core.DTO.ProductDTO
{
  public class ProductSalesDto
  {

    public Product? Product { get; set; }
    public int TotalSold { get; set; }
  }
}
