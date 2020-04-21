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
                        email = "JB@gmail.com",
                        average_mark = 68
                    },
                    new Student()
                    {
                        student_id = "132400L",
                        name = "Jane",
                        surname = "Said",
                        email = "JS@gmail.com",
                        average_mark = 65
                    }, new Student()
                    {
                        student_id = "432100L",
                        name = "Kyle",
                        surname = "West",
                        email = "KW@gmail.com",
                        average_mark = 78
                    }, new Student()
                    {
                        student_id = "123499M",
                        name = "Tom",
                        surname = "Olson",
                        email = "TO@gmail.com",
                        average_mark = 88
                    }, new Student()
                    {
                        student_id = "143200L",
                        name = "Ursula",
                        surname = "Borg",
                        email = "UB@gmail.com",
                        average_mark = 70
                    }, new Student()
                    {
                        student_id = "432199M",
                        name = "Link",
                        surname = "Vella",
                        email = "LV@gmail.com",
                        average_mark = 90
                    }, new Student()
                    {
                        student_id = "123498M",
                        name = "Ian",
                        surname = "Grech",
                        email = "IG@gmail.com",
                        average_mark = 68
                    }
                    
                ) ;
                context.SaveChanges();
            }
        }
    }
}
