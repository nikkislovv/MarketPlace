using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Server
{
    public class AdminInitializer
    {
        const string adminUserName= "Nikita";
        const string adminFullName= "Kislov";
        const string password = "7nikita444444";
        const string AdminEmail = "nikitakislov368rwr@gmail.com";
        const string adminPhoneNumber = "+37544562877241";
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("client") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("client"));
            }
            if (await roleManager.FindByNameAsync("seller") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }
            if (await userManager.FindByNameAsync(adminUserName) == null)
            {
                User admin = new User { UserName = adminUserName, Email=AdminEmail, FullName = adminFullName,PhoneNumber=adminPhoneNumber};
                var result= await userManager.CreateAsync(admin,password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");

                }
            }

        }
    }
}
