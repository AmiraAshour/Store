using Store.Core.DTO.Account;
using Store.Core.Entities;

namespace Store.Core.Interfaces
{
  public interface IAuthService
  {
    Task<AuthResultDTO> RegisterAsync(RegisterDTO model);
    Task<AuthResultDTO> LoginAsync(LoginDTO model);
    Task<bool> ConfirmEmailAsync(string userId, string token);
    Task<bool> ResendConfirmationEmailAsync(string email, string clientUrl);
    Task<bool> SendForgotPasswordEmailAsync(string email, string clientUrl);
    Task<AuthResultDTO?> ResetPasswordAsync(ResetPasswordDTO model);
    string GenerateAccessToken(AppUser user);
    Task<string> GenerateRefreshTokenAsync(AppUser user);
    Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken, string email);
    Task<bool> UpdateAddressAsync(string email, AddressDTO address);
    Task<Address?> GetAddressAsync(string email);

  }
}
