using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using FYPAllocationTest.Models;
using Microsoft.Extensions.DependencyInjection;


namespace FYPAllocationTest
{
    public class Program // This program deals with the building and executing of the application
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider; // Call ServiceProvider
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    DbInitializer.SeedRoles(roleManager); // Add roles on database creation
                    DbInitializer.SeedUsers(userManager); // Add test users to system
                }
                catch (Exception ex) // Log any errors that may occur during the seeding stage 
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured at the seeding stage");
                }

                host.Run();
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

        
}

