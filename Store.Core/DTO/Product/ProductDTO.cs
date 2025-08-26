using Store.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.DTO.Product
{
  public class ProductDTO
  {

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must be less than 100 characters.")]
    public string? Name { get; set; }

    [StringLength(300, ErrorMessage = "Description must be less than 300 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "New Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal NewPrice { get; set; }
    
    [Required(ErrorMessage = "Old Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal OldPrice { get; set; }

    [Required(ErrorMessage = "Category Name is required.")]
    public string?  CategoryName { get; set; }

    public  List<PhotoDTO> Photos { get; set; }
  }
}
