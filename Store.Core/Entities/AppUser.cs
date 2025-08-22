using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities
{
  public class AppUser:IdentityUser
  {
    public string DispalyName { get; set; }
    public Address Address { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
  }
}
