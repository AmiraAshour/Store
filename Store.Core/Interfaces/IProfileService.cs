

using Store.Core.DTO.Account;
using Store.Core.Entities;

namespace Store.Core.Interfaces
{
  public interface IProfileService
  {
    Task<ProfileDTO?> GetProfileAsync(string userId);
    Task<ProfileDTO?> UpdateProfileAsync(string userId, UpdateProfileDTO dto);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDTO dto);
    Task<bool> DeleteAccountAsync(string userId);
    IEnumerable<AppUser>? GetNewUsersForToday();
    IEnumerable<AppUser>? GetNewUsersForThisMonth();
  }
}
