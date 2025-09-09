using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Account;
using Store.Core.Interfaces;
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
    [HttpGet]
    public async Task<IActionResult> GetAllAddresses()
    {
      var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
      if(string.IsNullOrEmpty(userId))
        return ApiResponseHelper.Unauthorized("User is not authenticated");

      var addresses = await _addressService.GetAllAddressesAsync(userId);
      return ApiResponseHelper.Success(addresses, "Addresses retrieved successfully");
    }
    [HttpPost]
    public async Task<IActionResult> AddAddress([FromBody]AddressDTO addressDto)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId))
        return ApiResponseHelper.Unauthorized("User is not authenticated");

      var result = await _addressService.AddAddressAsync( addressDto,userId);
      if (result == null)
        return ApiResponseHelper.BadRequest("Failed to add address");
      return ApiResponseHelper.Created(result, "Address added successfully");
    }
    [HttpPut("{addressId}")]
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

    [HttpDelete("{addressId}")]
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
