

namespace Store.Core.DTO
{
  public record CategoryDTO(string Name ,string Description);
  public record UpdateCategoryDTO(string Name ,string Description,int Id);
 
}
