using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public interface IStudentRepository
    {
        public IEnumerable<Student> GetAllData();

        bool Import(Student student);
    }
}
