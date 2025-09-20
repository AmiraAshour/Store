namespace Store.Core.Entities.BasketEntity
{
  public class BasketItem
  {
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? CategoryName { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

  }
}