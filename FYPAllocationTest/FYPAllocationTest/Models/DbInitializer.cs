using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.student.Any())
            {
                context.AddRange(
                    new Student()
                    { 
                        student_id = "123400L",
                        name = "Joe",
                        surname = "Borg",
                        email = "JB@um.edu.mt",
                        average_mark = 68
                    },
                    new Student()
                    {
                        student_id = "132400L",
                        name = "Jane",
                        surname = "Said",
                        email = "JS@um.edu.mt",
                        average_mark = 65
                    }, new Student()
                    {
                        student_id = "432100L",
                        name = "Kyle",
                        surname = "West",
                        email = "KW@um.edu.mt",
                        average_mark = 78
                    }, new Student()
                    {
                        student_id = "123499M",
                        name = "Tom",
                        surname = "Olson",
                        email = "TO@um.edu.mt",
                        average_mark = 88
                    }, new Student()
                    {
                        student_id = "143200L",
                        name = "Ursula",
                        surname = "Borg",
                        email = "UB@um.edu.mt",
                        average_mark = 70
                    }, new Student()
                    {
                        student_id = "432199M",
                        name = "Link",
                        surname = "Vella",
                        email = "LV@um.edu.mt",
                        average_mark = 90
                    }, new Student()
                    {
                        student_id = "123498M",
                        name = "Ian",
                        surname = "Grech",
                        email = "IG@um.edu.mt",
                        average_mark = 68
                    }
                    
                ) ;
                context.SaveChanges();
            }
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Student")).Wait();
                roleManager.CreateAsync(new IdentityRole("Supervisor")).Wait();
                roleManager.CreateAsync(new IdentityRole("Administrator")).Wait();
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var student = new ApplicationUser()
                {
                    Email = "teststudent@testmail.com",
                    NormalizedEmail = "TESTSTUDENT@TESTMAIL.COM",
                    UserName = "teststudent@testmail.com",
                    NormalizedUserName = "TESTUSER@TESTMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };
                //Add to store
                IdentityResult result = userManager.CreateAsync(student, "FYPsRule!").Result;
                if (result.Succeeded)
                {
                    //Add to role
                    userManager.AddToRoleAsync(student, "Student").Wait();
                }

                var supervisor = new ApplicationUser()
                {
                    Email = "testsupervisor@testmail.com",
                    NormalizedEmail = "TESTSUPERVISOR@TESTMAIL.COM",
                    UserName = "testsupervisor@testmail.com",
                    NormalizedUserName = "TESTSUPERVISOR@TESTMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                //Add to store
                result = userManager.CreateAsync(supervisor, "FYPMaster!").Result;
                if (result.Succeeded)
                {
                    //Add to role
                    userManager.AddToRoleAsync(supervisor, "Supervisor").Wait();
                }


                var admin = new ApplicationUser()
                {
                    Email = "testadmin@testmail.com",
                    NormalizedEmail = "TESTADMIN@TESTMAIL.COM",
                    UserName = "testadmin@testmail.com",
                    NormalizedUserName = "TESTADMIN@TESTMAIL.COM",
                    PasswordHash = "Admin1234",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D") //to track important profile updates (e.g. password change)
                };

                //Add to store
                result = userManager.CreateAsync(admin, "FYPMaster!").Result;
                if (result.Succeeded)
                {
                    //Add to role
                    userManager.AddToRoleAsync(admin, "Administrator").Wait();
                }
            }
        }

    }
}
