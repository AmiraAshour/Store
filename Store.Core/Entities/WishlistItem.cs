

using Store.Core.Entities.ProductEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Core.Entities
{
  public class WishlistItem
  {

    public string UserId { get; set; } = string.Empty;

    public AppUser User { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
  }

}
