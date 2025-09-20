using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Store.API.Helper;
using Store.Core.DTO.Order;
using Store.Core.Entities.Order;
using System.Security.Claims;
using Store.Core.Interfaces.ServiceInterfaces;

namespace Store.API.Controllers
{
  [Authorize]
  public class OrdersController : BaseController
  {
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
      _orderService = orderService;
    }

    /// <summary>
    /// Create a new order for the logged-in user
    /// </summary>
    [HttpPost]
    [SwaggerOperation(
      Summary = "Create new order",
      Description = "Creates a new order for the authenticated user using their basket and delivery method."
    )]
    [ProducesResponseType(typeof(OrderToReturnDTO), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      if (email == null)
        return ApiResponseHelper.Unauthorized("Please login and try again");

      var order = await _orderService.CreateOrdersAsync(orderDTO, email);

      if (order == null)
        return ApiResponseHelper.BadRequest("Problem creating order");

      return ApiResponseHelper.Created(order, "Order created successfully");
    }

    /// <summary>
    /// Get all orders for the logged-in user
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
      Summary = "Get user orders",
      Description = "Returns all orders placed by the currently authenticated user."
    )]
    [ProducesResponseType(typeof(IEnumerable<OrderToReturnDTO>), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetOrdersForUser()
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var orders = await _orderService.GetAllOrdersForUserAsync(email!);
      return ApiResponseHelper.Success(orders);
    }

    /// <summary>
    /// Get details of a specific order by ID for the logged-in user
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerOperation(
      Summary = "Get order by ID",
      Description = "Fetches detailed information about a specific order for the authenticated user."
    )]
    [ProducesResponseType(typeof(Orders), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetOrderByIdForUser(int id)
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var order = await _orderService.GetOrderByIdAsync(id);
      if (order == null)
        return ApiResponseHelper.NotFound("Order not found");

      return ApiResponseHelper.Success(order);
    }

   
  }
}
