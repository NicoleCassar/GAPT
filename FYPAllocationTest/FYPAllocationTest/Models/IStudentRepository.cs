using System.Collections.Generic;

namespace FYPAllocationTest.Models
{
    public interface IStudentRepository // Interface connection StudentRepository to controller class
    { // Methods used are explained within the SutdentRepository class
        public IEnumerable<Student> GetAllData();
        bool Import(Student student);
    }
}
