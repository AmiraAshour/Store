using AutoMapper;
using Store.Core.DTO.Category;
using Store.Core.Entities.Product;

namespace Store.Core.Mapping
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
