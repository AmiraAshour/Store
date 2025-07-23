
using System.Net;
using System.Text.Json;

namespace Store.API.Middleware
{
  // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
      _next = next;
      _logger = logger;
      _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        string json;
      if(_env.IsDevelopment()){
          var response = new
          {
            status = httpContext.Response.StatusCode,
            message = ex.Message,
            stackTrace = ex.StackTrace
          };
         json = JsonSerializer.Serialize(response, options);
      }else{
          var response=  new
            {
              status = httpContext.Response.StatusCode,
              message = "Something went wrong. Please try again later.",
              stackTrace = ""
            };
          json = JsonSerializer.Serialize(response, options);
        }

        await httpContext.Response.WriteAsync(json);
      }
    }
  }

  // Extension method used to add the middleware to the HTTP request pipeline.
  public static class ExceptionMiddlewareExtensions
  {
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ExceptionMiddleware>();
    }
  }
}
