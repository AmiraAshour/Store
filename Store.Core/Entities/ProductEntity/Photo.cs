using Store.Core.Enitites;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Core.Entities.ProductEntity
{
  public class Photo:BaseEntity<int>
  {
    public string? ImageName { get; set; }
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; }

  }
}
