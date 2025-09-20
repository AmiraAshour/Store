using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Store.Core.DTO.Account;
using Store.Core.Entities.UserEntity;
using Store.Core.Interfaces.ServiceInterfaces;
using System.Threading.Tasks;

namespace Store.Core.Services
{
  public class ProfileSevice : IProfileService
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProfileSevice> _logger;

    public ProfileSevice(
        UserManager<AppUser> userManager,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<ProfileSevice> logger)
    {
      _userManager = userManager;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _logger = logger;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDTO dto)
    {
      _logger.LogInformation("User {UserId} attempting to change password.", userId);

      var user = await _userManager.FindByIdAsync(userId);
      if (user is null)
      {
        _logger.LogWarning("Change password failed. User {UserId} not found.", userId);
        return false;
      }

      var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
      if (result.Succeeded)
        _logger.LogInformation("Password changed successfully for user {UserId}.", userId);
      else
        _logger.LogError("Password change failed for user {UserId}. Errors: {Errors}", userId, result.Errors);

      return result.Succeeded;
    }

    public async Task<bool> DeleteAccountAsync(string userId)
    {
      _logger.LogInformation("Attempting to delete account for user {UserId}", userId);

      var user = await _userManager.FindByIdAsync(userId);
      if (user is null)
      {
        _logger.LogWarning("Delete failed. User {UserId} not found.", userId);
        return false;
      }

      var result = await _userManager.DeleteAsync(user);
      if (result.Succeeded)
        _logger.LogInformation("Account deleted successfully for user {UserId}", userId);
      else
        _logger.LogError("Failed to delete account for user {UserId}. Errors: {Errors}", userId, result.Errors);

      return result.Succeeded;
    }

    public async Task<ProfileDTO?> GetProfileAsync(string userId)
    {
      _logger.LogInformation("Fetching profile for user {UserId}", userId);

      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        _logger.LogWarning("Profile fetch failed. User {UserId} not found.", userId);
        return null;
      }

      var profile = _mapper.Map<ProfileDTO>(user);
      var address = await _unitOfWork.AddressRepository.GetAddressesAsync(userId);
      profile.Address = _mapper.Map<List<AddressDTO>>(address);

      _logger.LogInformation("Profile fetched successfully for user {UserId}", userId);
      return profile;
    }

    public async Task<ProfileDTO?> UpdateProfileAsync(string userId, UpdateProfileDTO dto)
    {
      _logger.LogInformation("Updating profile for user {UserId}", userId);

      var user = await _userManager.FindByIdAsync(userId);
      if (user is null)
      {
        _logger.LogWarning("Update failed. User {UserId} not found.", userId);
        return null;
      }

      user.DispalyName = dto.DispalyName;
      user.PhoneNumber = dto.PhoneNumber;

      var result = await _userManager.UpdateAsync(user);
      if (!result.Succeeded)
      {
        _logger.LogError("Failed to update profile for user {UserId}. Errors: {Errors}", userId, result.Errors);
        return null;
      }

      var profile = _mapper.Map<ProfileDTO>(user);
      var address = await _unitOfWork.AddressRepository.GetAddressesAsync(userId);
      profile.Address = _mapper.Map<List<AddressDTO>>(address);

      _logger.LogInformation("Profile updated successfully for user {UserId}", userId);
      return profile;
    }

    public IEnumerable<AppUser>? GetNewUsersForToday()
    {
      var today = DateTime.UtcNow.Date;
      _logger.LogInformation("Fetching new users for today: {Date}", today);

      var users = _userManager.Users.Where(u => u.CreatedAt.Date == today).ToList();
      _logger.LogInformation("Found {Count} new users today.", users.Count);

      return users;
    }

    public IEnumerable<AppUser>? GetNewUsersForThisMonth()
    {
      var now = DateTime.UtcNow;
      var firstDay = new DateTime(now.Year, now.Month, 1);
      var lastDay = firstDay.AddMonths(1).AddDays(-1);

      _logger.LogInformation("Fetching new users from {Start} to {End}", firstDay, lastDay);

      var users = _userManager.Users.Where(u => u.CreatedAt.Date >= firstDay && u.CreatedAt.Date <= lastDay).ToList();
      _logger.LogInformation("Found {Count} new users this month.", users.Count);

      return users;
    }

    public async Task<IEnumerable<string?>?> GetEmailsAdmin()
    {
      _logger.LogInformation("Fetching all admin emails.");

      var admins = await _userManager.GetUsersInRoleAsync("Admin");
      var emails = admins?.Select(a => a.Email);

      _logger.LogInformation("Found {Count} admin users.", admins?.Count ?? 0);
      return emails;
    }
  }
}
