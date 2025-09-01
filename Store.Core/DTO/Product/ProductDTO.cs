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

    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal NewPrice { get; set; }
    public decimal OldPrice { get; set; }
    public string?  CategoryName { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }

    public  List<PhotoDTO> Photos { get; set; }
  }
}
