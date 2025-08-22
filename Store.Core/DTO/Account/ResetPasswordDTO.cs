namespace Store.Core.DTO.Account
{
  public class ResetPasswordDTO
  {
    public string Email { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }
  }
}
