using Microsoft.AspNetCore.Http;
using Store.Core.Entities;

namespace Store.Core.Interfaces
{
  public interface IPaymentService
  {
    Task<PaymentIntentCreateResponse> CreateOrUpdateIntentAsync(int orderId, string buyerEmail);
    Task HandleWebhookAsync(HttpRequest request);
  }
}
