using Store.Core.Entities.Order;

namespace Store.Core.DTO.Order
{
  public class OrderToReturnDTO
  {
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public DateTime OrderDate { get; set; }
    public ShippingAddress shippingAddress { get; set; }
    public IReadOnlyList<OrderItemDTO> orderItems { get; set; }
    public DeliveryMethod deliveryMethod { get; set; }
    public string status { get; set; }
  }
}
