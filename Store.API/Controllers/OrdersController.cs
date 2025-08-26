using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Order;
using Store.Core.Interfaces;

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
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
    {
      var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
      if (email == null) 
        return ApiResponseHelper. Unauthrized("Please login and try again");

      var order = await _orderService.CreateOrdersAsync(orderDTO, email);

      if (order == null)
        return ApiResponseHelper. BadRequest("Problem creating order");

      return ApiResponseHelper.Created (order,"Oreder created successfuly");
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersForUser()
    {
      var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
      var orders = await _orderService.GetAllOrdersForUserAsync(email!);
      return ApiResponseHelper.Success(orders);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderByIdForUser(int id)
    {
      var email = HttpContext.User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
      var order = await _orderService.GetOrderByIdAsync(id, email!);
      if (order == null) 
        return ApiResponseHelper. NotFound("Order not found");

      return ApiResponseHelper.Success(order);
    }
    [HttpGet("deliveryMethods")]
    public async Task<IActionResult> GetDeliveryMethods()
    {
      var deliveryMethods = await _orderService.GetDeliveryMethodAsync();
      return ApiResponseHelper.Success(deliveryMethods);
    }
  }
}
