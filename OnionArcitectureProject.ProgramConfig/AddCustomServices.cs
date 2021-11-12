using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnionArcitectureProject.DataLayer.Context;

namespace OnionArcitectureProject.ProgramConfig
{
    public static class AddCustomServices
    {

        public static IServiceCollection AddCustomService(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;

                // Lockout settings
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            

            services.Configure<PasswordHasherOptions>(options =>
            {
                options.IterationCount = 12000;
            });

            

            return services;
        }

    }
}