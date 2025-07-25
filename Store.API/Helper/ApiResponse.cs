namespace Store.API.Helper
{
  public class ApiResponse<T>
  {
   
    private static string GetMessageFromStatusCode(int statusCode)
    {
      return statusCode switch
      {
        200 => "Success",
        201 => "Created",
        204 => "No Content",
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        500 => "Internal Server Error",
        _ => "Unknown Status Code"
      };
    }
    public int StatusCode { get; set; }
    public bool Success => StatusCode >= 200 && StatusCode < 300;
    public string? Message { get; set; } 
    public T? Data { get; set; }

    public ApiResponse(int statusCode, string? message = null, T? data = default)
    {
      StatusCode = statusCode;
      Message = message ?? GetMessageFromStatusCode(statusCode); ;
      Data = data;
    }
  }
}
