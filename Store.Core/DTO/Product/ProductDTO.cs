

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
