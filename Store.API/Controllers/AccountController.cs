
using Microsoft.AspNetCore.Mvc;
using Store.API.Controllers;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Interfaces;
public class AccountController : BaseController
{
  private readonly IAuthService _authService;

  public AccountController(IAuthService authService)
  {
    _authService = authService;
  }
  [HttpPost("register")]
  public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO? model)
  {
    if (model is null)
      return ApiResponseHelper.BadRequest("Model cannot be null");

    var result = await _authService.RegisterAsync(model);

    if (!result.Success)
      return ApiResponseHelper.BadRequest(string.Join(", ", result.Errors));
    return ApiResponseHelper.Created(result, "The account created");
  }

  [HttpPost("login")]
  public async Task<IActionResult> LoginAsync([FromBody] LoginDTO? model)
  {
    if (model is null)
      return ApiResponseHelper.BadRequest("Model cannot be null");
    var result = await _authService.LoginAsync(model);

    if (!result.Success)
      return ApiResponseHelper.Unauthrized(string.Join(", ", result.Errors));

    Response.Cookies.Append("token", result.AccessToken!, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTime.UtcNow.AddMinutes(30)
    });

    return ApiResponseHelper.Success(result, "Login successfuly");
  }

  [HttpGet("confirm-email")]
  public async Task<IActionResult> ConfirmEmailAsync(string email, string token)
  {
    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
      return ApiResponseHelper.BadRequest("Email and token are required");

    var result = await _authService.ConfirmEmailAsync(email, token);
    if (!result)
      return ApiResponseHelper.BadRequest("Email confirmation failed");
    return ApiResponseHelper.Success("", "Email confirmed successfully");
  }

  [HttpGet("resend-confirmation-email")]
  public async Task<IActionResult> ResendConfirmationEmailAsync(string email, string clientUrl)
  {
    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(clientUrl))
      return ApiResponseHelper.BadRequest("Email and client URL are required");
    var result = await _authService.ResendConfirmationEmailAsync(email, clientUrl);
    if (!result)
      return ApiResponseHelper.BadRequest("Resending confirmation email failed");
    return ApiResponseHelper.Success("", "Confirmation email resent successfully");
  }


  [HttpPost("forgot-password")]
  public async Task<IActionResult> ForgotPasswordAsync(string email, string clientUrl)
  {
    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(clientUrl))
      return ApiResponseHelper.BadRequest("Email and client URL are required");

    var result = await _authService.SendForgotPasswordEmailAsync(email, clientUrl);

    if (!result)
      return ApiResponseHelper.BadRequest("Sending forgot password email failed");
    return ApiResponseHelper.Success("", "Forgot password email sent successfully");
  }

  [HttpGet("resend-forgot-password-email")]
  public async Task<IActionResult> ResendForgotPasswordEmailAsync(string email, string clientUrl)
  {
    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(clientUrl))
      return ApiResponseHelper.BadRequest("Email and client URL are required");

    var result = await _authService.SendForgotPasswordEmailAsync(email, clientUrl);

    if (!result)
      return ApiResponseHelper.BadRequest("Resending forgot password email failed");
    return ApiResponseHelper.Success("", "Forgot password email resent successfully");
  }

  [HttpPost("reset-password")]
  public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDTO model)
  {
    if (model is null)
      return ApiResponseHelper.BadRequest("Model cannot be null");

    var result = await _authService.ResetPasswordAsync(model);

    if (result is null)
      return ApiResponseHelper.BadRequest("Reset password failed");
    if (!result.Success)
      return ApiResponseHelper.BadRequest(string.Join(", ", result.Errors));
    return Ok(result);

  }


  [HttpGet("refresh-token")]
  public async Task<IActionResult> RefreshToken(string email, string refreshToken)
  {
    var user = await _authService.GetUserByRefreshTokenAsync(refreshToken, email);
    if (user == null)
      return ApiResponseHelper.Unauthrized("Invalid refresh token");

    var newAccessToken = await _authService.GenerateAccessTokenAsync(user);
    var newRefreshToken = await _authService.GenerateRefreshTokenAsync(user);

    Response.Cookies.Append("token", newAccessToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTime.UtcNow.AddMinutes(30)
    });

    return ApiResponseHelper.Success(new
    {
      AccessToken = newAccessToken,
      RefreshToken = newRefreshToken
    }, "Access token added to cookies successfuly");
  }

  [HttpGet("logout")]
  public IActionResult Logout()
  {
    Response.Cookies.Delete("token");
    return ApiResponseHelper.Success("", "Logged out");
  }

  [HttpGet("IsUserAuth")]
  public IActionResult? IsUserAuth()
  {
    var user = User.Identity;
    if (user is null)
      return null;

    return user.IsAuthenticated ? Ok() : BadRequest();
  }

  [HttpPut("update-address")]
  public async Task<IActionResult> UpdateAddressAsync(AddressDTO? address)
  {
    if (address is null)
      return ApiResponseHelper.BadRequest("Address cannot be null");
    var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
    if (string.IsNullOrEmpty(email))
      return ApiResponseHelper.Unauthrized("User is not authenticated");
    var updatedAddress = await _authService.UpdateAddressAsync(email, address);
    if (updatedAddress)
      return ApiResponseHelper.BadRequest("Failed to update address");
    return ApiResponseHelper.Success(updatedAddress, "Address updated successfully");
  }

  [HttpGet("get-address")]
  public async Task<IActionResult> GetAddressAsync()
  {
    var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
    if (string.IsNullOrEmpty(email))
      return ApiResponseHelper.Unauthrized("User is not authenticated");
    var address = await _authService.GetAddressAsync(email);
    if (address is null)
      return ApiResponseHelper.NotFound("Address not found");
    return ApiResponseHelper.Success(address);
  }
}

