
using System.ComponentModel.DataAnnotations;

namespace Store.Core.DTO.CategoryEntityDTO
{
  public class CategoryDTO
  {
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(250, ErrorMessage = "Description cannot be longer than 250 characters.")]
    public string? Description { get; set; }
  }

}
