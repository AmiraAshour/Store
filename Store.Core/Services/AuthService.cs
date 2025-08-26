using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
    private readonly IConfiguration _config;
    private readonly IEmailService _emailSender;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthService(UserManager<AppUser> userManager,
                       SignInManager<AppUser> signInManager,
                       IConfiguration config,
                       IEmailService emailSender,
                       IUnitOfWork unitOfWork,
                       IMapper mapper)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _config = config;
      _emailSender = emailSender;
      _unitOfWork = unitOfWork;
      _mapper = mapper;
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

      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

      await _emailSender.SendConfirmationEmailAsync(user.Email, "Confirm your email",
          $"Please confirm your account by clicking this link", token, model.ClientUrl);

      return new AuthResultDTO { Success = true };
    }

    public async Task<AuthResultDTO> LoginAsync(LoginDTO model)
    {


      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null)
        return new AuthResultDTO { Success = false, Errors = new[] { "Invalid credentials" } };

      if (!user.EmailConfirmed)
      {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        await _emailSender.SendConfirmationEmailAsync(model.Email, "Confirm your email",
            $"Please confirm your account by clicking this link", token, model.ClientUrl ?? "");

        return new AuthResultDTO { Success = false, Errors = new[] { "Email not confirmed" } };
      }
      var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
      if (!result.Succeeded)
        return new AuthResultDTO { Success = false, Errors = new[] { "Invalid email or password" } };


      var accessToken = GenerateAccessToken(user);
      string  refreshToken =await GenerateRefreshTokenAsync(user);

      return new AuthResultDTO { Success = true, AccessToken = accessToken, RefreshToken = refreshToken };
    }


    public async Task<bool> ConfirmEmailAsync(string email, string token)
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null) return false;

      var result = await _userManager.ConfirmEmailAsync(user, token);
      return result.Succeeded;
    }
    public async Task<bool> ResendConfirmationEmailAsync(string email, string clientUrl)
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null || user.EmailConfirmed)
        return false;
      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      await _emailSender.SendConfirmationEmailAsync(email, "Confirm your email",
          $"Please confirm your account by clicking this link", token, clientUrl);
      return true;
    }

    public async Task<bool> SendForgotPasswordEmailAsync(string email, string clientUrl)
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        return false;

      var token = await _userManager.GeneratePasswordResetTokenAsync(user);

      await _emailSender.SendConfirmationEmailAsync(email, "Reset Password",
          $"Click here to reset your password", token, clientUrl);

      return true;
    }


    public async Task<AuthResultDTO?> ResetPasswordAsync(ResetPasswordDTO model)
    {
      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null)
        return null;

      var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
      if (result.Succeeded)
        return new AuthResultDTO { Success = true };

      return new AuthResultDTO { Success = false, Errors = result.Errors.Select(e => e.Description) };
    }


    public string GenerateAccessToken(AppUser user)
    {
      var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("displayName", user.DispalyName ?? "")
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddMinutes(15),
        Issuer = _config["Jwt:Issuer"],
        Audience = _config["Jwt:Audience"],
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(AppUser user)
    {
      var randomNumber = new byte[64];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);

      var token= Convert.ToBase64String(randomNumber);
      user.RefreshToken = token;
      user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

      await _userManager.UpdateAsync(user);

      return token;
    }

    public async Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken, string email)
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        return null;
      return user;
    }

    public async Task<bool> UpdateAddressAsync(string email, AddressDTO addressDTO)
    {
      var address=_mapper.Map<Address>(addressDTO);
      var existingAddress = await _unitOfWork.AddressRepository.GetAddressAsync(email);
      if (existingAddress is null)
      {
         address.AppUser.Email = email;
        return await _unitOfWork.AddressRepository.AddAddressAsync(address) is not null;
      }
      address.Id = existingAddress.Id;  
      return await _unitOfWork.AddressRepository.UpdateAddressAsync( address) is not null;
    }
    public async Task<Address?> GetAddressAsync(string email)
    {
      return await _unitOfWork.AddressRepository.GetAddressAsync(email);
    }

  }

}

