using Microsoft.AspNetCore.Identity;
using Store.Core.Entities;

namespace Store.API.Helper
{
  public class AppDbInitializer
  {
    public static async Task SeedRolesAndAdminAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {

      if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

      if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));

      var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
      if (adminUser == null)
      {
        var newAdmin = new AppUser()
        {
          DispalyName = "Admin User",
          UserName = "admin",
          Email = "admin@gmail.com",
          EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newAdmin, "Admin@123"); // باسورد افتراضي
        if (result.Succeeded)
        {
          await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
      }
    }
  }
}
