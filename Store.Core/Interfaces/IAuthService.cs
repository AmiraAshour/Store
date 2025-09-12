using Store.Core.DTO.Account;
using Store.Core.Entities;

namespace Store.Core.Interfaces
{
  public interface IAuthService
  {
    Task<AuthResultDTO> RegisterAsync(RegisterDTO model);
    Task<AuthResultDTO> LoginAsync(LoginDTO model);
    Task<bool> ConfirmEmailAsync(string userId, string token);
    Task<bool> ResendConfirmationEmailAsync(string email);
    Task<bool> SendForgotPasswordEmailAsync(string email);
    Task<AuthResultDTO?> ResetPasswordAsync(ResetPasswordDTO model);
    Task<string> GenerateAccessTokenAsync(AppUser user);
    Task<string> GenerateRefreshTokenAsync(AppUser user);
    Task<AuthResultDTO> HandleGoogleCallback();
    AppUser? GetUserByRefreshToken(string refreshToken);

  }
}
