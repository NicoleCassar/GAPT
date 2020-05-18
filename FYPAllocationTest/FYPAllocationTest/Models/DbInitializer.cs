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
    }
}
