
namespace Store.Core.DTO
{
  public class AddReviewDTO
  {
    public int ProductId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
  }
}
