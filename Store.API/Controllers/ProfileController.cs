
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
  [Authorize]
  public class ProfileController : BaseController
  {
    private readonly IProfileService _profileService;
    public ProfileController(IProfileService profileService)
    {
      _profileService = profileService;
    }
    [HttpGet("get-profile")]
    public async Task<IActionResult> GetProfile()
    {
      var userId = User.FindFirst("userId")?.Value;
      if (userId is null) return ApiResponseHelper.Unauthorized("Login and try again");

      var profile = await _profileService.GetProfileAsync(userId);

      if (profile is null) return ApiResponseHelper. NotFound("User not found");
      return ApiResponseHelper.Success(profile,"Profile retreve successfuly");
    }

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
    {
      var userId = User.FindFirst("userId")?.Value;
      if (userId is null) return ApiResponseHelper.Unauthorized("Login and try again");

      var updatedProfile = await _profileService.UpdateProfileAsync(userId, dto);
      if (updatedProfile is null) return ApiResponseHelper. NotFound("User not found");

      return ApiResponseHelper.Success(updatedProfile, "Profile updated successfuly");
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
    {
      var userId = User.FindFirst("userId")?.Value;
      if (userId is null) return ApiResponseHelper.Unauthorized("Login and try again");

      var result = await _profileService.ChangePasswordAsync(userId, dto);
      if (!result) return ApiResponseHelper. BadRequest("Password change failed. Please check your old password.");

      return ApiResponseHelper.Success("", "Password changed successfuly"); ;
    }

    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteAccount()
    {
      var userId = User.FindFirst("userId")?.Value;
      if (userId is null) ApiResponseHelper.Unauthorized("Login and try again");

      var result = await _profileService.DeleteAccountAsync(userId);
      if (!result) return ApiResponseHelper.BadRequest("Account deletion failed.");

      return ApiResponseHelper.Success("","Account deleted successfully.");
    }
  }
}
