using AcxiomCRM.Models;
using Microsoft.AspNetCore.Identity;

namespace AcxiomCRM.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
    {
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        var users = services.GetRequiredService<UserManager<ApplicationUser>>();
        foreach (var role in new[] { "Admin", "Manager", "SalesExecutive" })
            if (!await roles.RoleExistsAsync(role))
                await roles.CreateAsync(new IdentityRole(role));

        // Demo administrator. Change these credentials immediately after first run.
        const string email = "admin@acxiomcrm.local";
        var admin = await users.FindByEmailAsync(email);
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = email, Email = email, FullName = "CRM Administrator", EmailConfirmed = true, IsActive = true };
            var result = await users.CreateAsync(admin, "Admin@12345!");
            if (result.Succeeded) await users.AddToRoleAsync(admin, "Admin");
        }
    }
}
