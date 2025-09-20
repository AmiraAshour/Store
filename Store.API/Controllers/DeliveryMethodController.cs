using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Order;
using Store.Core.Entities.Order;
using Store.Core.Interfaces.ServiceInterfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Store.API.Controllers
{
  [Authorize]
  public class DeliveryMethodsController : BaseController
  {
    private readonly IDeliveryMethodServce _deliveryService;

    public DeliveryMethodsController(IDeliveryMethodServce deliveryService)
    {
      _deliveryService = deliveryService;
    }

    /// <summary>
    /// Get all available delivery methods (for users)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get delivery methods",
        Description = "Returns all available delivery methods that users can choose from when placing an order."
    )]
    [ProducesResponseType(typeof(IEnumerable<DeliveryMethod>), 200)]
    public async Task<IActionResult> GetAll()
    {
      var methods = await _deliveryService.GetDeliveryMethodAsync();
      return ApiResponseHelper.Success(methods);
    }

    /// <summary>
    /// Get a specific delivery method by ID (for users)
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get delivery method by ID",
        Description = "Fetches details of a single delivery method by its ID."
    )]
    [ProducesResponseType(typeof(DeliveryMethod), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
      var method = await _deliveryService.GetDeliveryMethodByIdAsync(id);
      if (method == null)
        return ApiResponseHelper.NotFound("Delivery method not found");

      return ApiResponseHelper.Success(method);
    }



    /// <summary>
    /// Add a new delivery method (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles ="Admin")]
    [SwaggerOperation(
        Summary = "Create delivery method",
        Description = "Allows admins to create a new delivery method."
    )]
    [ProducesResponseType(typeof(DeliveryMethod), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] DeliveryMethodDTO dto)
    {
      var created = await _deliveryService.AddDeliveryMethodAsync(dto);
      if (created == null)
        return ApiResponseHelper.BadRequest("Unable to create delivery method");

      return ApiResponseHelper.Created(created, "Delivery method created successfully");
    }


    /// <summary>
    /// Update an existing delivery method (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles ="Admin")]
    [SwaggerOperation(
        Summary = "Update delivery method",
        Description = "Allows admins to update details of an existing delivery method."
    )]
    [ProducesResponseType(typeof(DeliveryMethod), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] DeliveryMethodDTO dto)
    {
      var updated = await _deliveryService.UpdateDeliveryMethodAsync(id, dto);
      if (updated == null)
        return ApiResponseHelper.NotFound("Delivery method not found");

      return ApiResponseHelper.Success(updated, "Delivery method updated successfully");
    }

    /// <summary>
    /// Delete a delivery method (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]

    [SwaggerOperation(
        Summary = "Delete delivery method",
        Description = "Allows admins to delete an existing delivery method."
    )]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
      var deleted = await _deliveryService.DeleteDeliveryMethodAsync(id);
      if (!deleted)
        return ApiResponseHelper.NotFound("Delivery method not found");

      return ApiResponseHelper.NoContent("Delivery method deleted successfully");
    }
  }
}
