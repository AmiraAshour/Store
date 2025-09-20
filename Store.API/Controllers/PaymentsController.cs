using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.Interfaces.ServiceInterfaces;
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
