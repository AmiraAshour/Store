using Store.Core.Entities.BasketEntity;

namespace Store.Core.Entities.Order
{
  public class DeliveryMethod:BaseEntity<int>
  {
    public DeliveryMethod()
    {
      
    }
    public DeliveryMethod(string name, decimal price, string deliveryTime, string description)
    {
      Name = name;
      Price = price;
      DeliveryTime = deliveryTime;
      Description = description;
    }

    public string Name { get; set; }
    public decimal Price { get; set; }
    public string DeliveryTime { get; set; }
    public string Description { get; set; }
  }
}