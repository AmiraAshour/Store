
using Microsoft.Extensions.Caching.Memory;
using Store.API.Helper;
using System.Net;
using System.Text.Json;

namespace Store.API.Middleware
{
  // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
    {
      _next = next;
      _environment = environment;
      _memoryCache = memoryCache;
    }
    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        ApplySecurity(context);

        if (IsRequestAllowed(context) == false)
        {
          context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
          context.Response.ContentType = "application/json";

          var response = new
              ApiResponse<string>((int)HttpStatusCode.TooManyRequests, "Too many request. please try again later");

          await context.Response.WriteAsJsonAsync(response);
        }
        await _next(context);
      }
      catch (Exception ex)
      {

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var response = _environment.IsDevelopment() ?
            new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
            : new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message);

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
      }
    }
    private bool IsRequestAllowed(HttpContext context)
    {
      var ip = context.Connection.RemoteIpAddress?.ToString();
      var cachKey = $"Rate:{ip}";
      var dateNow = DateTime.Now;

      var (timesTamp, count) = _memoryCache.GetOrCreate(cachKey, entry =>
      {
        entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
        return (timesTamp: dateNow, count: 0);
      });

      if (dateNow - timesTamp < _rateLimitWindow)
      {
        if (count >= 80)
        {
          return false;
        }
        _memoryCache.Set(cachKey, (timesTamp, count += 1), _rateLimitWindow);
      }
      else
      {
        _memoryCache.Set(cachKey, (dateNow, count), _rateLimitWindow);
      }
      return true;
    }

    private void ApplySecurity(HttpContext context)
    {
      context.Response.Headers["X-Content-Type-Options"] = "nosniff";
      context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
      context.Response.Headers["X-Frame-Options"] = "DENY";

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
