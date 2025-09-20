using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Interfaces.ServiceInterfaces;
using Swashbuckle.AspNetCore.Annotations;

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

    /// <summary>
    /// Get the logged-in user's profile.
    /// </summary>
    /// <remarks>
    /// Returns user details including profile info and addresses.
    /// </remarks>
    /// <response code="200">Profile retrieved successfully</response>
    /// <response code="401">If user is not logged in</response>
    /// <response code="404">If user not found</response>
    [HttpGet("get-profile")]
    [SwaggerOperation(Summary = "Get user profile", Description = "Retrieves the current logged-in user's profile information.")]
    public async Task<IActionResult> GetProfile()
    {
      var userId = User.FindFirst("userId")?.Value;
      if (userId is null) return ApiResponseHelper.Unauthorized("Login and try again");

      var profile = await _profileService.GetProfileAsync(userId);

      if (profile is null) return ApiResponseHelper.NotFound("User not found");
      return ApiResponseHelper.Success(profile, "Profile retrieved successfully");
    }

    /// <summary>
    /// Update the logged-in user's profile.
    /// </summary>
    /// <param name="dto">Profile update details (Display Name, Phone Number, etc.)</param>
    /// <response code="200">Profile updated successfully</response>
    /// <response code="401">If user is not logged in</response>
    /// <response code="404">If user not found</response>
    [HttpPut("update-profile")]
    [SwaggerOperation(Summary = "Update user profile", Description = "Updates the current logged-in user's profile information.")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
    {
      var userId = User.FindFirst("userId")?.Value;
      if (userId is null) return ApiResponseHelper.Unauthorized("Login and try again");

      var updatedProfile = await _profileService.UpdateProfileAsync(userId, dto);
      if (updatedProfile is null) return ApiResponseHelper.NotFound("User not found");

      return ApiResponseHelper.Success(updatedProfile, "Profile updated successfully");
    }

    /// <summary>
    /// Change the logged-in user's password.
    /// </summary>
    /// <param name="dto">Old password and new password</param>
    /// <response code="200">Password changed successfully</response>
    /// <response code="400">If old password is incorrect or update failed</response>
    /// <response code="401">If user is not logged in</response>
    [HttpPut("change-password")]
    [SwaggerOperation(Summary = "Change password", Description = "Changes the password of the current logged-in user.")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
    {
      var userId = User.FindFirst("userId")?.Value;
      if (userId is null) return ApiResponseHelper.Unauthorized("Login and try again");

      var result = await _profileService.ChangePasswordAsync(userId, dto);
      if (!result) return ApiResponseHelper.BadRequest("Password change failed. Please check your old password.");

      return ApiResponseHelper.Success("", "Password changed successfully");
    }

    /// <summary>
    /// Delete the logged-in user's account.
    /// </summary>
    /// <remarks>
    /// Permanently deletes the user's account and related data.
    /// </remarks>
    /// <response code="200">Account deleted successfully</response>
    /// <response code="400">If deletion failed</response>
    /// <response code="401">If user is not logged in</response>
    [HttpDelete("delete-account")]
    [SwaggerOperation(Summary = "Delete account", Description = "Deletes the current logged-in user's account permanently.")]
    public async Task<IActionResult> DeleteAccount()
    {
      var userId = User.FindFirst("userId")?.Value;
      if (userId is null) ApiResponseHelper.Unauthorized("Login and try again");

      var result = await _profileService.DeleteAccountAsync(userId);
      if (!result) return ApiResponseHelper.BadRequest("Account deletion failed.");

      return ApiResponseHelper.Success("", "Account deleted successfully.");
    }
  }
}
