using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.API.Controllers;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Entities;
using Store.Core.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
public class AccountController : BaseController
{
  private readonly IAuthService _authService;
  private readonly IConfiguration _config;
  private readonly SignInManager<AppUser> _signInManager;

  public AccountController(IAuthService authService, IConfiguration config, SignInManager<AppUser> signInManager)
  {
    _authService = authService;
    _config = config;
    _signInManager = signInManager;
  }
  /// <summary>Register a new user</summary>
  [HttpPost("register")]
  [SwaggerOperation(Summary = "Register a new user", Description = "Creates a new user account using email, password, DisplayName, Username and send email confirmation to this email.")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO? model)
  {
    if (model is null)
      return ApiResponseHelper.BadRequest("Model cannot be null");

    var result = await _authService.RegisterAsync(model);

    if (!result.Success)
      return ApiResponseHelper.BadRequest(string.Join(", ", result.Errors));

    return ApiResponseHelper.Created(result, "The account created");
  }

  /// <summary>User login</summary>
  [HttpPost("login")]
  [SwaggerOperation(Summary = "User login", Description = "Logs in user with email and password, returns JWT tokens and refresh token.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    return ApiResponseHelper.Success(new { result.AccessToken, result.RefreshToken }, "Login successfuly");
  }

  /// <summary>Confirm user email</summary>
  [HttpGet("confirm-email")]
  [SwaggerOperation(Summary = "Confirm email", Description = "Confirms a user's email using token sent in email.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
  {
    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
      return ApiResponseHelper.BadRequest("Email and token are required");

    var result = await _authService.ConfirmEmailAsync(userId, token);
    if (!result)
      return ApiResponseHelper.BadRequest("Email confirmation failed");

    return ApiResponseHelper.Success("", "Email confirmed successfully");
  }

  /// <summary>Resend confirmation email</summary>
  [HttpGet("resend-confirmation-email")]
  [SwaggerOperation(Summary = "Resend confirmation email", Description = "Resends email confirmation link to user.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ResendConfirmationEmailAsync(string email)
  {
    if (string.IsNullOrEmpty(email))
      return ApiResponseHelper.BadRequest("Email and client URL are required");

    var result = await _authService.ResendConfirmationEmailAsync(email);

    if (!result)
      return ApiResponseHelper.BadRequest("Resending confirmation email failed");

    return ApiResponseHelper.Success("", "Confirmation email resent successfully");
  }

  /// <summary>Send forgot password email</summary>
  [HttpPost("forgot-password-email")]
  [SwaggerOperation(Summary = "Forgot password email", Description = "Sends a password reset email to the user.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ForgotPasswordAsync(string email)
  {
    if (string.IsNullOrEmpty(email))
      return ApiResponseHelper.BadRequest("Email and client URL are required");

    var result = await _authService.SendForgotPasswordEmailAsync(email);

    if (!result)
      return ApiResponseHelper.BadRequest("Sending forgot password email failed");

    return ApiResponseHelper.Success("", "Forgot password email sent successfully");
  }

  /// <summary>Resend forgot password email</summary>
  [HttpPost("resend-forgot-password-email")]
  [SwaggerOperation(Summary = "Resend forgot password email", Description = "Resends the password reset email to the user.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ResendForgotPasswordEmailAsync(string email)
  {
    if (string.IsNullOrEmpty(email))
      return ApiResponseHelper.BadRequest("Email and client URL are required");

    var result = await _authService.SendForgotPasswordEmailAsync(email);

    if (!result)
      return ApiResponseHelper.BadRequest("Resending forgot password email failed");

    return ApiResponseHelper.Success("", "Forgot password email resent successfully");
  }

  /// <summary>Reset user password</summary>
  [HttpPost("reset-password")]
  [SwaggerOperation(Summary = "Reset password", Description = "Resets user password using reset token sent by email.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

  /// <summary>Refresh access token</summary>
  [HttpPost("refresh-token")]
  [SwaggerOperation(Summary = "Refresh token", Description = "Generates new access and refresh tokens using the current refresh token.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

  /// <summary>User logout</summary>
  [HttpGet("logout")]
  [Authorize]
  [SwaggerOperation(Summary = "Logout", Description = "Removes JWT token from cookies and logs out the user.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public IActionResult Logout()
  {
    Response.Cookies.Delete("token");
    return ApiResponseHelper.Success("", "Logged out");
  }

  /// <summary>Google login (external)</summary>
  [HttpGet("externallogin/google")]
  [SwaggerOperation(Summary = "Google login", Description = "Redirects user to Google login page for external authentication.")]
  [ProducesResponseType(StatusCodes.Status302Found)]
  public IActionResult ExternalLoginGoogle()
  {
    var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
    var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
    return Challenge(properties, "Google");
  }

  /// <summary>Google login callback</summary>
  [HttpGet("externallogin/google-callback")]
  [SwaggerOperation(Summary = "Google login callback", Description = "Handles the callback from Google login and returns JWT tokens.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ExternalLoginCallback()
  {
    var result = await _authService.HandleGoogleCallback();
    if (result.Errors != null) return BadRequest(new { result.Errors });

    Response.Cookies.Append("token", result.AccessToken!, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:EXPIRATION_MINUTES"]))
    });

    return ApiResponseHelper.Success(new { result.AccessToken, result.RefreshToken }, "Login successfuly");
  }

  /// <summary>Check if user is authenticated</summary>
  [HttpGet("IsUserAuth")]
  [SwaggerOperation(Summary = "Check user authentication", Description = "Checks if the current request has a valid authenticated user.")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public IActionResult? IsUserAuth()
  {
    var user = User.Identity;
    if (user is null || !user.IsAuthenticated)
      return ApiResponseHelper.Unauthorized("User is not authenticated");

    return ApiResponseHelper.Success("", "User is authenticated");
  }

}

