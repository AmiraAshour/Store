using Store.Core.Entities.BasketEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Core.Entities.Order
{
  public class Orders : BaseEntity<int>
  {
    public Orders()
    {

    }
    public Orders(string buyerEmail, decimal subTotal, ShippingAddress shippingAddress, string paymentIntentId, List<OrderItem> orderItems, int deliveryMethodId)
    {
      BuyerEmail = buyerEmail;
      SubTotal = subTotal;
      this.shippingAddress = shippingAddress;
      PaymentIntentId = paymentIntentId;
      this.orderItems = orderItems;
      this.DeliveryMethodId = deliveryMethodId;
    }

    public string BuyerEmail { get; set; }
    public decimal SubTotal { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public ShippingAddress shippingAddress { get; set; }
    public string PaymentIntentId { get; set; }
    public List<OrderItem> orderItems { get; set; }

    [ForeignKey(nameof(DeliveryMethodId))]
    public DeliveryMethod deliveryMethod { get; set; }
    public int DeliveryMethodId { get; set; }


    public Status status { get; set; } = Status.Pending;

    public decimal GetTotal()
    {
      return SubTotal + deliveryMethod.Price;
    }

  }
}