using Store.Core.Entities.BasketEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Core.Entities.ProductEntity
{
  public class Product:BaseEntity<int>
  {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public virtual List<Photo> Photos { get; set; }
    public int CategoryId { get; set; }

    [ForeignKey(nameof (CategoryId))]
    public virtual Category Category { get; set; }
    public double AverageRating { get; set; }   
    public int ReviewCount { get; set; }
  }
}
