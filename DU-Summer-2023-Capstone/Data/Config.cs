using Microsoft.AspNetCore.Identity;

namespace DU_Summer_2023_Capstone.Data
{
    public class Config
    {
        public static async Task<bool> CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            Task<IdentityResult> roleResult;
            string email = "admin@domain.com";

            //Check that there is an Administrator role and create if not
            Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Admin");
            hasAdminRole.Wait();

            if (!hasAdminRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Admin"));
                roleResult.Wait();
            }


            //Check if the admin user exists and create it if not
            //Add to the Administrator role

            Task<IdentityUser> testUser = userManager.FindByEmailAsync(email);
            testUser.Wait();

            if (testUser.Result == null)
            {
                IdentityUser administrator = new IdentityUser();
                administrator.Email = email;
                administrator.UserName = email;
                administrator.EmailConfirmed = true;



                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, "Pizzares1@Password");
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Admin");
                    newUserRole.Wait();
                    return true;
                }
            }

            return false;
        }

    }
}
