using AutoMapper;
using Store.Core.DTO;
using Store.Core.Entities.Product;

namespace Store.Core.Mapping
{
  public class ProductMapping:Profile
  {
    public ProductMapping() {
      CreateMap<ProductDTO,Product>().ReverseMap();
      CreateMap<UpdateProductDTO,Product>().ReverseMap();
    }
  }
}
