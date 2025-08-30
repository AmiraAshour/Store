using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.Interfaces;
using Stripe;
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


    [HttpGet("create-intent/{OrderId}")]
    public async Task<IActionResult> CreateIntent( int OrderId)
    { 
      var email = User.FindFirstValue(ClaimTypes.Email);
      if (email is null) return ApiResponseHelper.Unauthrized("Login first");

      var res = await _payments.CreateOrUpdateIntentAsync(OrderId ,email);
      return ApiResponseHelper.Success(res, "Client secret created");
    }

    // TEST-ONLY: confirm on server (handy for Postman). DO NOT use in production.
    [HttpPost("pay-now-test")]
    public async Task<IActionResult> PayNowTest([FromBody] int OrderId)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      if (email is null) return ApiResponseHelper.Unauthrized("Login first");

      // ensure PI exists
      var create = await _payments.CreateOrUpdateIntentAsync(OrderId, email);

      var service = new PaymentIntentService();
      var confirm = await service.ConfirmAsync(create.PaymentIntentId, new PaymentIntentConfirmOptions
      {
        PaymentMethod = "pm_card_visa" // Stripe test card
      });

      return ApiResponseHelper.Success(new { confirm.Id, confirm.Status });
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
      await _payments.HandleWebhookAsync(Request);
      return Ok();
    }
  }
}

