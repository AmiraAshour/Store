using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Store.API.Helper
{
  public static class ApiResponseHelper
  {
    public static IActionResult Success<T>(T? data, string? message = null) =>
        new OkObjectResult(new ApiResponse<T>((int)HttpStatusCode.OK, message, data));

    public static IActionResult Created<T>(T data, string? message = null) =>
        new ObjectResult(new ApiResponse<T>((int)HttpStatusCode.Created, message, data)) { StatusCode = 201 };

    public static IActionResult NotFound(string? message = null) =>
        new NotFoundObjectResult(new ApiResponse<string>((int)HttpStatusCode.NotFound, message));

    public static IActionResult BadRequest(string? message = null) =>
        new BadRequestObjectResult(new ApiResponse<string>((int)HttpStatusCode.BadRequest, message));
    public static IActionResult Unauthrized(string? message = null) =>
        new UnauthorizedObjectResult(new ApiResponse<string>((int)HttpStatusCode.Unauthorized, message));
  }

}
