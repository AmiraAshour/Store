using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.Interfaces;
using Stripe;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Store.API.Controllers
{
  [Authorize]
  public class PaymentsController : BaseController
  {
    private readonly IPaymentService _payments;

    public PaymentsController(IPaymentService payments)
    {
      _payments = payments;
    }

    /// <summary>
    /// Create or update a Stripe Payment Intent for a specific order.
    /// </summary>
    /// <remarks>
    /// This endpoint generates a PaymentIntent on Stripe and returns the ClientSecret  
    /// that the client app can use to confirm payment.
    /// </remarks>
    /// <param name="OrderId">The ID of the order to pay for</param>
    /// <response code="200">Returns the PaymentIntent client secret</response>
    /// <response code="401">If the user is not logged in</response>
    [HttpGet("create-intent/{OrderId}")]
    [SwaggerOperation(Summary = "Create Stripe Payment Intent",
                      Description = "Generates or updates a PaymentIntent in Stripe and attaches it to the given order.")]
    public async Task<IActionResult> CreateIntent(int OrderId)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      if (email is null) return ApiResponseHelper.Unauthorized("Login first");

      var res = await _payments.CreateOrUpdateIntentAsync(OrderId, email);
      return ApiResponseHelper.Success(res, "Client secret created");
    }

    /// <summary>
    /// Test-only endpoint to confirm a payment immediately.
    /// </summary>
    /// <remarks>
    /// ⚠️ For development/testing only. Uses Stripe's test card (`pm_card_visa`).  
    /// DO NOT use in production environments.
    /// </remarks>
    /// <param name="OrderId">The ID of the order to simulate payment for</param>
    /// <response code="200">Returns the PaymentIntent confirmation result</response>
    [HttpPost("pay-now-test")]
    [SwaggerOperation(Summary = "Simulate Payment Confirmation (Test Only)",
                      Description = "Confirms a PaymentIntent immediately using Stripe’s test card. Not for production use.")]
    public async Task<IActionResult> PayNowTest([FromBody] int OrderId)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      if (email is null) return ApiResponseHelper.Unauthorized("Login first");

      // ensure PI exists
      var create = await _payments.CreateOrUpdateIntentAsync(OrderId, email);

      var service = new PaymentIntentService();
      var confirm = await service.ConfirmAsync(create.PaymentIntentId, new PaymentIntentConfirmOptions
      {
        PaymentMethod = "pm_card_visa" // Stripe test card
      });

      return ApiResponseHelper.Success(new { confirm.Id, confirm.Status });
    }

    /// <summary>
    /// Stripe Webhook endpoint for receiving payment events.
    /// </summary>
    /// <remarks>
    /// This endpoint listens to Stripe webhooks such as:  
    /// - `payment_intent.succeeded`  
    /// - `payment_intent.payment_failed`  
    ///  
    /// It updates the corresponding order’s payment status accordingly.
    /// </remarks>
    /// <response code="200">Acknowledges receipt of the webhook event</response>
    [AllowAnonymous]
    [HttpPost("webhook")]
    [SwaggerOperation(Summary = "Stripe Webhook Listener",
                      Description = "Handles Stripe webhook events (payment success/failure) and updates order status.")]
    public async Task<IActionResult> Webhook()
    {
      await _payments.HandleWebhookAsync(Request);
      return Ok();
    }
  }
}
