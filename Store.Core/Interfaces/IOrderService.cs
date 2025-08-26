using Store.Core.DTO.Order;
using Store.Core.Entities.Order;

namespace Store.Core.Interfaces
{
  public interface IOrderService
  {
    Task<OrderToReturnDTO> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail);
    Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail);
    Task<OrderToReturnDTO?> GetOrderByIdAsync(int Id, string BuyerEmail);
    Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodAsync();
  }
}
