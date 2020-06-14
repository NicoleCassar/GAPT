using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FYPAllocationTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace FYPAllocationTest
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration _configuration { get; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        // This method gets called by the runtime and used to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment())
            { // Add services to specify each required repository
                services.AddTransient<IStudentRepository, StudentRepository>();
                services.AddTransient<ISupervisorRepository, SupervisorRepository>();
                services.AddTransient<IAreaRepository, AreaRepository>();
                services.AddTransient<IPreferenceRepository, PreferenceRepository>();
                services.AddTransient<IAllocationRepository, AllocationRepository>();
            }
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")) // Connect to SQL server
            );
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //Password policy
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                //Lockout settings
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                //User settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789-._@+";

            }).AddEntityFrameworkStores<AppDbContext>();
                services.AddControllersWithViews().AddRazorRuntimeCompilation();
            }

        // This method gets called by the runtime and used to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection(); // Enable redirection
            app.UseStatusCodePagesWithRedirects("/Home/Error"); // Redirect to custom error page
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication(); // Add authentication
            app.UseAuthorization(); // Add authorization functionality

            app.UseEndpoints(endpoints =>
            { // Map to error page on 404 and home page on loading the system
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "404-PageNotFound",
                    pattern: "{controller=Home}/{action=Error}/{id?}");

            });
        }
    }
}
