using Store.Core.Entities.BasketEntity;
using System.ComponentModel.DataAnnotations;

namespace Store.Core.Entities.comman
{
  public class Review:BaseEntity<int>
  {
    public int ProductId { get; set; }  
    public string BuyerEmail { get; set; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }     
    public string? Comment { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}
