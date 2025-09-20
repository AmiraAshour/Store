using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Interfaces.ServiceInterfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Store.API.Controllers
{
  [Authorize]
  public class AddressesController : BaseController
  {
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
      _addressService = addressService;
    }
    /// <summary>
    /// Get all addresses of the authenticated user.
    /// </summary>
    /// <returns>List of addresses for the logged-in user.</returns>
    [HttpGet]
    [SwaggerOperation(
      Summary = "Retrieve all user addresses",
      Description = "Returns a list of addresses associated with the authenticated user."
    )]
    public async Task<IActionResult> GetAllAddresses()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId))
        return ApiResponseHelper.Unauthorized("User is not authenticated");

      var addresses = await _addressService.GetAllAddressesAsync(userId);
      return ApiResponseHelper.Success(addresses, "Addresses retrieved successfully");
    }

    /// <summary>
    /// Add a new address for the authenticated user.
    /// </summary>
    /// <param name="addressDto">The address information to add.</param>
    /// <returns>Newly created address.</returns>
    [HttpPost]
    [SwaggerOperation(
      Summary = "Add new address",
      Description = "Creates a new address and assigns it to the authenticated user."
    )]
    public async Task<IActionResult> AddAddress([FromBody] AddressDTO addressDto)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId))
        return ApiResponseHelper.Unauthorized("User is not authenticated");

      var result = await _addressService.AddAddressAsync(addressDto, userId);
      if (result == null)
        return ApiResponseHelper.BadRequest("Failed to add address");
      return ApiResponseHelper.Created(result, "Address added successfully");
    }

    /// <summary>
    /// Update an existing address of the authenticated user.
    /// </summary>
    /// <param name="addressId">The ID of the address to update.</param>
    /// <param name="addressDto">Updated address information.</param>
    /// <returns>Updated address details.</returns>
    [HttpPut("{addressId}")]
    [SwaggerOperation(
      Summary = "Update address",
      Description = "Updates the details of an existing address belonging to the authenticated user."
    )]
    public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] AddressDTO addressDto)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId))
        return ApiResponseHelper.Unauthorized("User is not authenticated");

      var updatedAddress = await _addressService.UpdateAddressAsync(addressId, addressDto, userId);

      if (updatedAddress == null)
        return ApiResponseHelper.NotFound("Address not found or failed to update");
      return ApiResponseHelper.Success(updatedAddress, "Address updated successfully");
    }

    /// <summary>
    /// Delete an address of the authenticated user.
    /// </summary>
    /// <param name="addressId">The ID of the address to delete.</param>
    /// <returns>Status of the deletion operation.</returns>
    [HttpDelete("{addressId}")]
    [SwaggerOperation(
      Summary = "Delete address",
      Description = "Deletes an existing address if it belongs to the authenticated user."
    )]
    public async Task<IActionResult> DeleteAddress(int addressId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId))
        return ApiResponseHelper.Unauthorized("User is not authenticated");
      var success = await _addressService.DeleteAddressAsync(addressId, userId);

      if (!success)
        return ApiResponseHelper.NotFound("Failed to delete");
      return ApiResponseHelper.Success("", "Address deleted successfully");
    }

  }
}
