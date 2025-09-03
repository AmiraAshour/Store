using AutoMapper;
using Store.Core.DTO.CategoryEntityDTO;
using Store.Core.Entities.ProductEntity;

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
