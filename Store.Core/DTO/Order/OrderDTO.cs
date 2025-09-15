

namespace Store.Core.DTO.Order
{

  public class OrderDTO
  {
    public int deliveryMethodId { get; set; }

    public string basketId { get; set; }
    public ShipAddressDTO shipAddress { get; set; }
  }
}

