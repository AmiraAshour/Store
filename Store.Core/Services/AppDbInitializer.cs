using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Store.Core.Entities.UserEntity;

namespace Store.API.Helper
{
  public class AppDbInitializer
  {
    public static async Task SeedRolesAndAdminAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,IConfiguration configuration)
    {

      if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

      if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));

      var adminUser = await userManager.FindByEmailAsync(configuration["AdminUser:Email"]!);
      if (adminUser == null)
      {
        var newAdmin = new AppUser()
        {
          DispalyName = "Admin User",
          UserName = "admin",
          Email = configuration["AdminUser:Email"]!,
          EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newAdmin, configuration["AdminUser:Password"]!); 
        if (result.Succeeded)
        {
          await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
      }
    }
  }
}
