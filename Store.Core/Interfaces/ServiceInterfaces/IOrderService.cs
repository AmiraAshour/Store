using Store.Core.DTO.Order;
using Store.Core.Entities.Order;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IOrderService
  {
    Task<OrderToReturnDTO> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail);
    Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail);
    Task<Orders?> GetOrderByIdAsync(int Id);
    Task AttachPaymentIntentAsync(int Id, string PaymentIntentId);

    Task MarkOrderAsPaidAsync(int orderId, string paymentIntentId);
    Task MarkOrderAsFailedAsync(int orderId, string paymentIntentId);

    Task<IEnumerable<Orders>?> GetOrdersForTodayAsync();
    Task<IEnumerable<Orders>?> GetOrdersForThisMonthAsync();
    Task<IEnumerable<Orders>?> GetAllOrderUnDeliveredAsunc();
  }
}
