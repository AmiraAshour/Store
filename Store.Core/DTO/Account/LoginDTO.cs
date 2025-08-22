namespace Store.Core.DTO.Account
{
  public class LoginDTO
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public string? ClientUrl { get; set; }
  }
}
