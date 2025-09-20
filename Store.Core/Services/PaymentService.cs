using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Store.Core.Entities.comman;
using Store.Core.Entities.EntitySettings;
using Store.Core.Interfaces.ServiceInterfaces;
using Stripe;
using System.Text;

namespace Store.Core.Services
{
  public class PaymentService : IPaymentService
  {
    private readonly IOrderService _orderService;
    private readonly StripeSettings _stripe;
    private readonly ILogger<PaymentService> _logger;

    private const string PaymentIntentSucceeded = "payment_intent.succeeded";
    private const string PaymentIntentPaymentFailed = "payment_intent.payment_failed";

    public PaymentService(
        IOrderService orderService,
        IOptions<StripeSettings> stripe,
        ILogger<PaymentService> logger)
    {
      _orderService = orderService;
      _stripe = stripe.Value;
      _logger = logger;
    }

    public async Task<PaymentIntentCreateResponse> CreateOrUpdateIntentAsync(int orderId, string buyerEmail)
    {
      _logger.LogInformation("Starting payment intent process for OrderId: {OrderId}, BuyerEmail: {BuyerEmail}", orderId, buyerEmail);

      var order = await _orderService.GetOrderByIdAsync(orderId);
      if (order is null)
      {
        _logger.LogError("Order {OrderId} not found or not owned by user", orderId);
        throw new ArgumentException("Order not found or not owned by user");
      }

      var total = order.SubTotal + (order.deliveryMethod?.Price ?? 0);
      var amount = (long)(total * 100m);

      _logger.LogInformation("Calculated total for Order {OrderId}: {Total} ({Amount} in cents)", order.Id, total, amount);

      var intentService = new PaymentIntentService();
      PaymentIntent intent;

      if (string.IsNullOrEmpty(order.PaymentIntentId))
      {
        _logger.LogInformation("Creating new PaymentIntent for Order {OrderId}", order.Id);

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

        _logger.LogInformation("Created PaymentIntent {PaymentIntentId} for Order {OrderId}", intent.Id, order.Id);
      }
      else
      {
        _logger.LogInformation("Updating existing PaymentIntent {PaymentIntentId} for Order {OrderId}", order.PaymentIntentId, order.Id);

        var update = new PaymentIntentUpdateOptions { Amount = amount };
        intent = await intentService.UpdateAsync(order.PaymentIntentId, update);

        _logger.LogInformation("Updated PaymentIntent {PaymentIntentId} with new amount {Amount}", order.PaymentIntentId, amount);
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
      _logger.LogInformation("Handling Stripe webhook request");

      request.EnableBuffering();
      using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
      var json = await reader.ReadToEndAsync();
      request.Body.Position = 0;

      try
      {
        var signature = request.Headers["Stripe-Signature"];
        var stripeEvent = EventUtility.ConstructEvent(
            json,
            signature,
            _stripe.WebhookSecret,
            throwOnApiVersionMismatch: false);

        _logger.LogInformation("Received Stripe event: {EventType}", stripeEvent.Type);

        if (stripeEvent.Type == PaymentIntentSucceeded)
        {
          var intent = (PaymentIntent)stripeEvent.Data.Object;
          if (intent.Metadata.TryGetValue("orderId", out var str) && int.TryParse(str, out var orderId))
          {
            _logger.LogInformation("PaymentIntent {PaymentIntentId} succeeded for Order {OrderId}", intent.Id, orderId);
            await _orderService.MarkOrderAsPaidAsync(orderId, intent.Id);
          }
        }
        else if (stripeEvent.Type == PaymentIntentPaymentFailed)
        {
          var intent = (PaymentIntent)stripeEvent.Data.Object;
          if (intent.Metadata.TryGetValue("orderId", out var str) && int.TryParse(str, out var orderId))
          {
            _logger.LogWarning("PaymentIntent {PaymentIntentId} failed for Order {OrderId}", intent.Id, orderId);
            await _orderService.MarkOrderAsFailedAsync(orderId, intent.Id);
          }
        }
        else
        {
          _logger.LogWarning("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
        }
      }
      catch (StripeException ex)
      {
        _logger.LogError(ex, "StripeException occurred while handling webhook");
        throw;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Unexpected error occurred while handling webhook");
        throw;
      }
    }
  }
}
