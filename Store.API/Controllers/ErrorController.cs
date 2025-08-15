
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;

namespace Store.API.Controllers
{
  [Route("errors/{StatusCode}")]
  [ApiController]
  public class ErrorController : ControllerBase
  {
    [HttpGet]
    public IActionResult Error(int statusCode)
    {
      return new ObjectResult(new ApiResponse<ErrorController>(statusCode));

    }
  }
}
