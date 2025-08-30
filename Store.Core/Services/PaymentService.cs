using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Store.Core.Entities;
using Store.Core.Entities.EntitySettings;
using Store.Core.Interfaces;
using Stripe;

namespace Store.Core.Services
{
  public class PaymentService:IPaymentService
  {
    private readonly IOrderService _orderService; // you already have this service
    private readonly StripeSettings _stripe;
    // Add these constants at the top of the PaymentService class (or in a suitable location within the class)
    private const string PaymentIntentSucceeded = "payment_intent.succeeded";
    private const string PaymentIntentPaymentFailed = "payment_intent.payment_failed";
    public PaymentService(IOrderService orderService, IOptions<StripeSettings> stripe)
    {
      _orderService = orderService;
      _stripe = stripe.Value;
    }

    public async Task<PaymentIntentCreateResponse> CreateOrUpdateIntentAsync(int orderId, string buyerEmail)
    {
      var order = await _orderService.GetOrderByIdAsync(orderId);
      if (order is null) throw new ArgumentException("Order not found or not owned by user");

      // Calculate total (ALWAYS on server)
      var total = order.SubTotal + (order.deliveryMethod?.Price ?? 0);
      var amount = (long)(total * 100m); // stripe uses smallest unit

      var intentService = new PaymentIntentService();

      PaymentIntent intent;
      if (string.IsNullOrEmpty(order.PaymentIntentId))
      {
        var create = new PaymentIntentCreateOptions
        {
          Amount = amount,
          Currency = _stripe.Currency,
          AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions { Enabled = true },
          ReceiptEmail = buyerEmail,
          Metadata = new Dictionary<string, string>
          {
            ["orderId"] = order.Id.ToString(),
            ["buyerEmail"] = buyerEmail
          }
        };

        intent = await intentService.CreateAsync(create, new RequestOptions { IdempotencyKey = order.Id.ToString() });
        await _orderService.AttachPaymentIntentAsync(order.Id, intent.Id);
      }
      else
      {
        var update = new PaymentIntentUpdateOptions { Amount = amount };
        intent = await intentService.UpdateAsync(order.PaymentIntentId, update);
      }

      return new PaymentIntentCreateResponse
      {
        ClientSecret = intent.ClientSecret,
        PaymentIntentId = intent.Id,
        PublishableKey = _stripe.PublishableKey
      };
    }

    public async Task HandleWebhookAsync(HttpRequest request)
    {
      // Read raw body
      request.EnableBuffering();
      using var reader = new StreamReader(request.Body, leaveOpen: true);
      var json = await reader.ReadToEndAsync();
      request.Body.Position = 0;

      try
      {
        var signature = request.Headers["Stripe-Signature"];
        var stripeEvent = EventUtility.ConstructEvent(json, signature, _stripe.WebhookSecret);

        // Replace the usage of 'Events.PaymentIntentSucceeded' and 'Events.PaymentIntentPaymentFailed' in HandleWebhookAsync with the new constants
        if (stripeEvent.Type == PaymentIntentSucceeded)
        {
          var intent = (PaymentIntent)stripeEvent.Data.Object;
          if (intent.Metadata.TryGetValue("orderId", out var str) && int.TryParse(str, out var orderId))
          {
            await _orderService.MarkOrderAsPaidAsync(orderId, intent.Id);
          }
        }
        else if (stripeEvent.Type == PaymentIntentPaymentFailed)
        {
          var intent = (PaymentIntent)stripeEvent.Data.Object;
          if (intent.Metadata.TryGetValue("orderId", out var str) && int.TryParse(str, out var orderId))
          {
            await _orderService.MarkOrderAsFailedAsync(orderId, intent.Id);
          }
        }
       
      }
      catch (StripeException ex)
      {
        // log ex; then rethrow/ignore based on your policy
        throw;
      }
    }
  }
}
