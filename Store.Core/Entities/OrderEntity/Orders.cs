using Store.Core.Entities.BasketEntity;

namespace Store.Core.Entities.Order
{
  public class Orders:BaseEntity<int>
  {
    public Orders()
    {
      
    }
    public Orders( string buyerEmail, decimal subTotal, ShippingAddress shippingAddress, string paymentIntentId, IReadOnlyList<OrderItem> orderItems, DeliveryMethod deliveryMethod)
    {
      BuyerEmail = buyerEmail;
      SubTotal = subTotal;
      this.shippingAddress = shippingAddress;
      PaymentIntentId = paymentIntentId;
      this.orderItems = orderItems;
      this.deliveryMethod = deliveryMethod;
    }

    public string BuyerEmail { get; set; }
    public decimal SubTotal { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public ShippingAddress shippingAddress { get; set; }
    public string PaymentIntentId { get; set; }
    public IReadOnlyList<OrderItem> orderItems { get; set; }
    public DeliveryMethod deliveryMethod { get; set; }


    public Status status { get; set; } = Status.Pending;

    public decimal GetTotal()
    {
      return SubTotal + deliveryMethod.Price;
    }

  }
}
