using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Entities.UserEntity;
using Store.Core.Interfaces.ServiceInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Store.Core.Services
{
  public class AuthService : IAuthService
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<AppUser> userManager,
                       SignInManager<AppUser> signInManager,
                       IConfiguration config,
                       RoleManager<IdentityRole> roleManager,
                       ILogger<AuthService> logger)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _config = config;
      _roleManager = roleManager;
      _logger = logger;
    }

    public async Task<AuthResultDTO> RegisterAsync(RegisterDTO model)
    {
      _logger.LogInformation("Registering new user with email: {Email}", model.Email);

      var emailExists = await _userManager.FindByEmailAsync(model.Email);
      if (emailExists is not null)
      {
        _logger.LogWarning("Registration failed: Email {Email} already exists.", model.Email);
        return new AuthResultDTO { Success = false, Errors = new[] { "Email already exists" } };
      }
      var userNameExists = await _userManager.FindByNameAsync(model.UserName);
      if (userNameExists is not null)
      {
        _logger.LogWarning("Registration failed: Username {UserName} already exists.", model.UserName);
        return new AuthResultDTO { Success = false, Errors = new[] { " Username already exists" } };
      }
      var user = new AppUser
      {
        UserName = model.UserName,
        Email = model.Email,
        DispalyName = model.DisplayName
      };

      var result = await _userManager.CreateAsync(user, model.Password);

      if (!result.Succeeded)
      {
        _logger.LogWarning("Registration failed for user {Email}: {Errors}", model.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
        return new AuthResultDTO { Success = false, Errors = result.Errors.Select(e => e.Description) };
      }

      await AppDbInitializer.SeedRolesAndAdminAsync(_userManager, _roleManager, _config);
      await _userManager.AddToRoleAsync(user, "User");

      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

      _logger.LogInformation("User {Email} registered successfully. Sending confirmation email.", model.Email);
      BackgroundJob.Enqueue<IEmailService>(_emailSender => _emailSender.SendConfirmationEmailAsync(user.Email, user.Id, token, _config["FrontendUrls:ConfirmEmail"]!));

      return new AuthResultDTO { Success = true };
    }

    public async Task<AuthResultDTO> LoginAsync(LoginDTO model)
    {
      _logger.LogInformation("User login attempt for email: {Email}", model.Email);
      await AppDbInitializer.SeedRolesAndAdminAsync(_userManager, _roleManager,_config);

      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null)
      {
        _logger.LogWarning("Login failed: Invalid credentials for email {Email}", model.Email);
        return new AuthResultDTO { Success = false, Errors = new[] { "Invalid credentials" } };
      }

      if (!user.EmailConfirmed)
      {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        _logger.LogWarning("Login failed: Email {Email} not confirmed. Sending confirmation email.", model.Email);
        BackgroundJob.Enqueue<IEmailService>(_emailSender => _emailSender.SendConfirmationEmailAsync(user.Email, user.Id, token, _config["FrontendUrls:ConfirmEmail"]!));

        return new AuthResultDTO { Success = false, Errors = new[] { "Email not confirmed chick your email to confirm" } };
      }
      var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
      if (!result.Succeeded)
      {
        _logger.LogWarning("Login failed: Invalid password for email {Email}", model.Email);
        return new AuthResultDTO { Success = false, Errors = new[] { "Invalid email or password" } };
      }

      var accessToken = await GenerateAccessTokenAsync(user);
      string refreshToken = await GenerateRefreshTokenAsync(user);

      _logger.LogInformation("User {Email} logged in successfully.", model.Email);
      return new AuthResultDTO { Success = true, AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
      _logger.LogInformation("Confirming email for userId: {UserId}", userId);
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        _logger.LogWarning("Email confirmation failed: UserId {UserId} not found.", userId);
        return false;
      }

      var result = await _userManager.ConfirmEmailAsync(user, token);
      _logger.LogInformation("Email confirmation for userId {UserId}: {Result}", userId, result.Succeeded);
      return result.Succeeded;
    }

    public async Task<bool> ResendConfirmationEmailAsync(string email)
    {
      _logger.LogInformation("Resending confirmation email to {Email}", email);
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null)
      {
        _logger.LogWarning("Resend confirmation failed: User {Email} not found.", email);
        throw new Exception("The user with the provided email does not exist.");
      }

      if (user.EmailConfirmed)
      {
        _logger.LogWarning("Resend confirmation failed: Email {Email} already confirmed.", email);
        throw new Exception("Email is allready confiermed");
      }

      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      BackgroundJob.Enqueue<IEmailService>(_emailSender => _emailSender.SendConfirmationEmailAsync(user.Email, user.Id, token, _config["FrontendUrls:ConfirmEmail"]!));
      _logger.LogInformation("Confirmation email resent to {Email}", email);

      return true;
    }

    public async Task<bool> SendForgotPasswordEmailAsync(string email)
    {
      _logger.LogInformation("Sending forgot password email to {Email}", email);
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null)
      {
        _logger.LogWarning("Forgot password failed: User {Email} not found.", email);
        throw new Exception("The user with the provided email does not exist.");
      }

      if (!(await _userManager.IsEmailConfirmedAsync(user)))
      {
        _logger.LogWarning("Forgot password failed: Email {Email} not confirmed. Resending confirmation email.", email);
        await ResendConfirmationEmailAsync(email);
        throw new Exception("Email is not confirmed please confirm email and try again");
      }

      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
      BackgroundJob.Enqueue<IEmailService>(_emailSender => _emailSender.SendResetPasswordEmailAsync(user.Email, user.Id, token, _config["FrontendUrls:ConfirmEmail"]!));
      _logger.LogInformation("Forgot password email sent to {Email}", email);

      return true;
    }

    public async Task<AuthResultDTO?> ResetPasswordAsync(ResetPasswordDTO model)
    {
      _logger.LogInformation("Resetting password for userId: {UserId}", model.UserId);
      var user = await _userManager.FindByIdAsync(model.UserId);
      if (user == null)
      {
        _logger.LogWarning("Reset password failed: UserId {UserId} not found.", model.UserId);
        return null;
      }

      var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
      _logger.LogInformation("Password reset for userId {UserId}: {Result}", model.UserId, result.Succeeded);
      if (result.Succeeded)
        return new AuthResultDTO { Success = true };

      return new AuthResultDTO { Success = false, Errors = result.Errors.Select(e => e.Description) };
    }

    public async Task<string> GenerateAccessTokenAsync(AppUser user)
    {
      _logger.LogInformation("Generating access token for userId: {UserId}", user.Id);
      DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:EXPIRATION_MINUTES"]));

      var claims = new List<Claim> {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id!),
        new Claim(ClaimTypes.Name, user.DispalyName),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim("UserId",user.Id.ToString())
      };
      var roles = await _userManager.GetRolesAsync(user);
      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }

      SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      JwtSecurityToken tokenGenerator = new JwtSecurityToken(
        _config["Jwt:Issuer"],
        _config["Jwt:Audience"],
        claims,
        expires: expiration,
        signingCredentials: signingCredentials
      );

      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.WriteToken(tokenGenerator);
      _logger.LogInformation("Access token generated for userId: {UserId}", user.Id);
      return token;
    }

    public async Task<string> GenerateRefreshTokenAsync(AppUser user)
    {
      _logger.LogInformation("Generating refresh token for userId: {UserId}", user.Id);
      var randomNumber = new byte[64];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);

      var token = Convert.ToBase64String(randomNumber);
      user.RefreshToken = token;
      user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

      await _userManager.UpdateAsync(user);
      _logger.LogInformation("Refresh token generated and saved for userId: {UserId}", user.Id);

      return token;
    }

    public AppUser? GetUserByRefreshToken(string refreshToken)
    {
      _logger.LogInformation("Getting user by refresh token.");
      var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);
      if (user == null)
        _logger.LogWarning("No user found for the provided refresh token.");
      return user;
    }

    public async Task<AuthResultDTO> HandleGoogleCallback()
    {
      _logger.LogInformation("Handling Google external login callback.");
      await AppDbInitializer.SeedRolesAndAdminAsync(_userManager, _roleManager,_config);

      var info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        _logger.LogWarning("Google external login info not found.");
        return new AuthResultDTO() { Success = false, Errors = new[] { "External login info not found" } };
      }

      var signInResult = await _signInManager.ExternalLoginSignInAsync(
          info.LoginProvider, info.ProviderKey, false, true);

      AppUser? user = null;

      if (signInResult.Succeeded)
      {
        user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        _logger.LogInformation("Google external login succeeded for user {Email}", user?.Email);
      }
      else
      {
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (email == null)
        {
          _logger.LogWarning("Email not found from Google provider.");
          return new AuthResultDTO()
          {
            Success = false,
            Errors = new[] { "Email not found from provider" }
          };
        }

        user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
          user = new AppUser
          {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            DispalyName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email
          };
          var result = await _userManager.CreateAsync(user);
          if (!result.Succeeded)
          {
            _logger.LogWarning("Failed to create user for Google login: {Email}", email);
            return new AuthResultDTO()
            {
              Success = false,
              Errors = new[] { "Failed to create user" }
            };
          }
        }

        await _userManager.AddToRoleAsync(user, "User");

        var addLogin = await _userManager.AddLoginAsync(user,
            new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.ProviderDisplayName));

        if (!addLogin.Succeeded)
        {
          _logger.LogWarning("Failed to link external login for user {Email}", email);
          return new AuthResultDTO()
          {
            Success = false,
            Errors = new[] { "Failed to link external login" }
          };
        }
      }

      var accessToken = await GenerateAccessTokenAsync(user);
      var refreshToken = await GenerateRefreshTokenAsync(user);

      _logger.LogInformation("Google login completed for user {Email}", user?.Email);
      return new AuthResultDTO() { Success = true, AccessToken = accessToken, RefreshToken = refreshToken };
    }
  }
}

