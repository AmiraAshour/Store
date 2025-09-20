using Microsoft.AspNetCore.Http;
using Store.Core.Entities.comman;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IPaymentService
  {
    Task<PaymentIntentCreateResponse> CreateOrUpdateIntentAsync(int orderId, string buyerEmail);
    Task HandleWebhookAsync(HttpRequest request);
  }
}
