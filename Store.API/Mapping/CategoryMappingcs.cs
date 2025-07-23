using AutoMapper;
using Store.Core.DTO;
using Store.Core.Entities.Product;

namespace Store.API.Mapping
{
  public class CategoryMappingcs:Profile
  {
    public CategoryMappingcs()
    {
      CreateMap<CategoryDTO,Category>().ReverseMap();
      CreateMap<UpdateCategoryDTO,Category>().ReverseMap();
    }
  }
}
