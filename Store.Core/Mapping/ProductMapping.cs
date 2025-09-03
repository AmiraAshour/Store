using AutoMapper;
using Store.Core.DTO.Product;
using Store.Core.Entities.ProductEntity;

namespace Store.Core.Mapping
{
  public class ProductMapping:Profile
  {
    public ProductMapping() {
      CreateMap<Product, ProductDTO>()
        .ForMember(x => x.CategoryName,
        op => op.MapFrom(src => src.Category.Name))
        .ReverseMap();
      
      CreateMap<Photo, PhotoDTO>().ReverseMap();

      CreateMap<AddProductDTO,Product>()
        .ForMember(d => d.Photos, op => op.Ignore()).ReverseMap();

      CreateMap<UpdateProductDTO,Product>()
        .ForMember(d=>d.Photos,s=>s.Ignore()).ReverseMap();
    }
  }
}
