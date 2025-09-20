using Store.Core.DTO.Account;
using Store.Core.Entities.UserEntity;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IProfileService
  {
    Task<ProfileDTO?> GetProfileAsync(string userId);
    Task<ProfileDTO?> UpdateProfileAsync(string userId, UpdateProfileDTO dto);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDTO dto);
    Task<bool> DeleteAccountAsync(string userId);
    IEnumerable<AppUser>? GetNewUsersForToday();
    IEnumerable<AppUser>? GetNewUsersForThisMonth();
    Task<IEnumerable<string?>?> GetEmailsAdmin();
  }
}
