
using Microsoft.AspNetCore.Mvc;
using Store.API.Controllers;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Interfaces;
public class AccountController : BaseController
{
  private readonly IAuthService _authService;
  private readonly IConfiguration _config;

  public AccountController(IAuthService authService,IConfiguration config)
  {
    _authService = authService;
    _config = config;
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
      return ApiResponseHelper.Unauthorized(string.Join(", ", result.Errors));

    Response.Cookies.Append("token", result.AccessToken!, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:EXPIRATION_MINUTES"]))
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
  public async Task<IActionResult> ResendConfirmationEmailAsync(string email)
  {
    if (string.IsNullOrEmpty(email))
      return ApiResponseHelper.BadRequest("Email and client URL are required");
    var result = await _authService.ResendConfirmationEmailAsync(email);

    if (!result)
      return ApiResponseHelper.BadRequest("Resending confirmation email failed");
    return ApiResponseHelper.Success("", "Confirmation email resent successfully");
  }


  [HttpPost("forgot-password-email")]
  public async Task<IActionResult> ForgotPasswordAsync(string email)
  {
    if (string.IsNullOrEmpty(email) )
      return ApiResponseHelper.BadRequest("Email and client URL are required");

    var result = await _authService.SendForgotPasswordEmailAsync(email);

    if (!result)
      return ApiResponseHelper.BadRequest("Sending forgot password email failed");
    return ApiResponseHelper.Success("", "Forgot password email sent successfully");
  }

  [HttpPost("resend-forgot-password-email")]
  public async Task<IActionResult> ResendForgotPasswordEmailAsync(string email)
  {
    if (string.IsNullOrEmpty(email))
      return ApiResponseHelper.BadRequest("Email and client URL are required");

    var result = await _authService.SendForgotPasswordEmailAsync(email);

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


  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
  {
    var user = _authService.GetUserByRefreshToken(refreshToken);
    if (user == null)
      return ApiResponseHelper.Unauthorized("Invalid refresh token");

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

  //[HttpGet("profile")]
  //public async Task<IActionResult> GetProfileAsync()
  //{
  //  var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
  //  if (string.IsNullOrEmpty(email))
  //    return ApiResponseHelper.Unauthrized("User is not authenticated");

  //  var profile = await _authService.GetProfileAsync(email);
  //  if (profile is null)
  //    return ApiResponseHelper.NotFound("Profile not found");

  //  return ApiResponseHelper.Success(profile, "Profile retrieved successfully");
  //}

  //[HttpPut("update-profile")]
  //public async Task<IActionResult> UpdateProfileAsync(UpdateProfileDTO model)
  //{
  //  if (model is null)
  //    return ApiResponseHelper.BadRequest("Profile cannot be null");

  //  var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
  //  if (string.IsNullOrEmpty(email))
  //    return ApiResponseHelper.Unauthrized("User is not authenticated");

  //  var updated = await _authService.UpdateProfileAsync(email, model);
  //  return updated
  //      ? ApiResponseHelper.Success("", "Profile updated successfully")
  //      : ApiResponseHelper.BadRequest("Failed to update profile");
  //}


  [HttpGet("IsUserAuth")]
  public IActionResult? IsUserAuth()
  {
    var user = User.Identity;
    if (user is null || !user.IsAuthenticated)
      return ApiResponseHelper.Unauthorized("User is not authenticated");

    return ApiResponseHelper.Success("", "User is authenticated");
  }

  
}

