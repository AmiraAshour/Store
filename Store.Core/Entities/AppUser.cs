using Microsoft.AspNetCore.Identity;

namespace Store.Core.Entities
{
  public class AppUser:IdentityUser
  {
    public string DispalyName { get; set; }
    public List< Address> Address { get; set; }

    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}
