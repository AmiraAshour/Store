using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Entities;
using Store.Core.Interfaces;
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

    public AuthService(UserManager<AppUser> userManager,
                       SignInManager<AppUser> signInManager,
                       IConfiguration config,
                       RoleManager<IdentityRole> roleManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _config = config;
      _roleManager = roleManager;
    }

    public async Task<AuthResultDTO> RegisterAsync(RegisterDTO model)
    {

      var emailExists = await _userManager.FindByEmailAsync(model.Email);
      if (emailExists is not null)
      {
        return new AuthResultDTO { Success = false, Errors = new[] { "Email already exists" } };
      }
      var userNameExists = await _userManager.FindByNameAsync(model.UserName);
      if (userNameExists is not null)
      {
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
        return new AuthResultDTO { Success = false, Errors = result.Errors.Select(e => e.Description) };

      await AppDbInitializer.SeedRolesAndAdminAsync(_userManager, _roleManager);

      await _userManager.AddToRoleAsync(user, "User");

      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

     BackgroundJob.Enqueue<IEmailService>( _emailSender => _emailSender.SendConfirmationEmailAsync(user.Email, user.Id,token, _config["FrontendUrls:ConfirmEmail"]!));

      return new AuthResultDTO { Success = true };
    }

    public async Task<AuthResultDTO> LoginAsync(LoginDTO model)
    {
      await AppDbInitializer.SeedRolesAndAdminAsync(_userManager, _roleManager);


      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null)
        return new AuthResultDTO { Success = false, Errors = new[] { "Invalid credentials" } };

      if (!user.EmailConfirmed)
      {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        BackgroundJob.Enqueue<IEmailService>(_emailSender => _emailSender.SendConfirmationEmailAsync(user.Email, user.Id, token, _config["FrontendUrls:ConfirmEmail"]!));

        return new AuthResultDTO { Success = false, Errors = new[] { "Email not confirmed chick your email to confirm" } };
      }
      var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
      if (!result.Succeeded)
        return new AuthResultDTO { Success = false, Errors = new[] { "Invalid email or password" } };


      var accessToken = await GenerateAccessTokenAsync(user);
      string refreshToken = await GenerateRefreshTokenAsync(user);

      return new AuthResultDTO { Success = true, AccessToken = accessToken, RefreshToken = refreshToken };
    }


    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null) return false;

      var result = await _userManager.ConfirmEmailAsync(user, token);
      return result.Succeeded;
    }
    public async Task<bool> ResendConfirmationEmailAsync(string email)
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null )
        throw new Exception("The user with the provided email does not exist.");

      if (user.EmailConfirmed)
        throw new Exception("Email is allready confiermed");

      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      BackgroundJob.Enqueue<IEmailService>(_emailSender => _emailSender.SendConfirmationEmailAsync(user.Email, user.Id, token, _config["FrontendUrls:ConfirmEmail"]!));

      return true;
    }

    public async Task<bool> SendForgotPasswordEmailAsync(string email)
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null )
        throw new Exception("The user with the provided email does not exist.");

      if(!(await _userManager.IsEmailConfirmedAsync(user))){
        await ResendConfirmationEmailAsync(email);
        throw new Exception("Email is not confirmed please confirm email and try again");
      }

      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
      BackgroundJob.Enqueue<IEmailService>(_emailSender => _emailSender.SendResetPasswordEmailAsync(user.Email, user.Id, token, _config["FrontendUrls:ConfirmEmail"]!));

      return true;
    }


    public async Task<AuthResultDTO?> ResetPasswordAsync(ResetPasswordDTO model)
    {
      var user = await _userManager.FindByIdAsync(model.UserId);
      if (user == null)
        return null;

      var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
      if (result.Succeeded)
        return new AuthResultDTO { Success = true };

      return new AuthResultDTO { Success = false, Errors = result.Errors.Select(e => e.Description) };
    }


    public async Task<string> GenerateAccessTokenAsync(AppUser user)
    {
      DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:EXPIRATION_MINUTES"]));

      var claims = new List<Claim> {
     new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //Subject (user id)
     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique ID
     new Claim(ClaimTypes.NameIdentifier, user.Id!), //Unique name identifier of the user (Email)
     new Claim(ClaimTypes.Name, user.DispalyName), //Name of the user
     new Claim(ClaimTypes.Email, user.Email!) ,//Name of the user
     new Claim("UserId",user.Id.ToString()) //Custom claim to store user ID
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
      return tokenHandler.WriteToken(tokenGenerator);

    }

    public async Task<string> GenerateRefreshTokenAsync(AppUser user)
    {
      var randomNumber = new byte[64];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);

      var token = Convert.ToBase64String(randomNumber);
      user.RefreshToken = token;
      user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

      await _userManager.UpdateAsync(user);

      return token;
    }

    public AppUser? GetUserByRefreshToken(string refreshToken)
    {

      var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

      return user;
    }


    public async Task<AuthResultDTO> HandleGoogleCallback()
    {
      await AppDbInitializer.SeedRolesAndAdminAsync(_userManager, _roleManager);

      var info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
        return new AuthResultDTO() { Success = false, Errors = new[] { "External login info not found" } };

      var signInResult = await _signInManager.ExternalLoginSignInAsync(
          info.LoginProvider, info.ProviderKey, false, true);

      AppUser? user = null;

      if (signInResult.Succeeded)
      {
        user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
      }
      else
      {
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (email == null)
          return new AuthResultDTO()
          {
            Success = false,
            Errors = new[] { "Email not found from provider" }
          };

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
          if (!result.Succeeded) return new AuthResultDTO()
          {
            Success = false,
            Errors = new[] { "Failed to create user" }
          };
        }

        await _userManager.AddToRoleAsync(user, "User");

        var addLogin = await _userManager.AddLoginAsync(user,
            new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.ProviderDisplayName));

        if (!addLogin.Succeeded) return new AuthResultDTO()
        {
          Success = false,
          Errors = new[] { "Failed to link external login" }
        };
      }

      var accessToken = await GenerateAccessTokenAsync(user);
      var refreshToken = await GenerateRefreshTokenAsync(user);

      return new AuthResultDTO() { Success = true, AccessToken = accessToken, RefreshToken = refreshToken };
    }

  }

}

