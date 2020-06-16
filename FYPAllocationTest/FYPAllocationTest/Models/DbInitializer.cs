using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace FYPAllocationTest.Models
{
    public class DbInitializer // This class allows for the seeding of roles and prepared users to be used for testing puposes
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager) // Apply roles to used across the system
        {
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Student")).Wait();  // Create Student role
                roleManager.CreateAsync(new IdentityRole("Supervisor")).Wait(); // Create Supervisor role
                roleManager.CreateAsync(new IdentityRole("Administrator")).Wait(); // Create Administrator role
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager) // Seed users to be utilised for testing
        {
            if (!userManager.Users.Any()) // Adding user to take on the role of a student
            {
                var student = new ApplicationUser()
                {
                    Id = "313699M",
                    Email = "connor.sant.17@um.edu.mt",
                    NormalizedEmail = "CONNOR.SANT.17@UM.EDU.MT",
                    UserName = "connor.sant.17@um.edu.mt",
                    NormalizedUserName = "CONNOR.SANT.17@UM.EDU.MT",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };
                IdentityResult result = userManager.CreateAsync(student, "FYPsRule!").Result; // Creating the user with the provided credentials
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(student, "Student").Wait(); // Add to role
                }

                var supervisor = new ApplicationUser() // Creating user to be assigned the role of supervisor
                {
                    Id = "123464M",
                    Email = "jabela@um.edu.mt",
                    NormalizedEmail = "JABELA@UM.EDU.MT",
                    UserName = "jabela@um.edu.mt",
                    NormalizedUserName = "JABELA@UM.EDU.MT",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };
                result = userManager.CreateAsync(supervisor, "FYPMaster!").Result; // Creation of user with credentials provided
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(supervisor, "Supervisor").Wait(); // Add to role
                }


                var admin = new ApplicationUser() // Seeding user to serve the role of administrator 
                {
                    Email = "secretary@um.edu.mt",
                    NormalizedEmail = "SECRETARY@UM.EDU.MT",
                    UserName = "secretary@um.edu.mt",
                    NormalizedUserName = "SECRETARY@UM.EDU.MT",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };
                result = userManager.CreateAsync(admin, "FYPMaster!").Result; // Creating user with supplied credentials 
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Administrator").Wait(); // Add to role
                }
            }
        }

    }
}
