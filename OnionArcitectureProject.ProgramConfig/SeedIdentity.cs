using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnionArcitectureProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcitectureProject.ProgramConfig
{
    public class SeedIdentity
    {
        

        public async Task AddSeedDataIdentity(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            try
            {
                // seed Roles
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Writer.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));


                // seed Admin User
                var admin = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "behnamasaei@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                if (userManager.Users.All(u => u.Id != admin.Id))
                {
                    var user = await userManager.FindByEmailAsync(admin.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(admin, "123456");
                        await userManager.AddToRoleAsync(admin, Roles.Basic.ToString());
                        await userManager.AddToRoleAsync(admin, Roles.Writer.ToString());
                        await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
                        await userManager.AddToRoleAsync(admin, Roles.SuperAdmin.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
