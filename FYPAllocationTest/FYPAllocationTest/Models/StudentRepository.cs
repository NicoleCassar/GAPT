using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FYPAllocationTest.Models
{
    public class StudentRepository : IStudentRepository // This repository serves to hold methods that communicate with the 'student' table
    {
        private readonly AppDbContext _appDbContext;
        public StudentRepository(AppDbContext appDbContext) // Constructor for repository
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Student> GetAllData() // Get all student data
        {
            return _appDbContext.student;
        }
        public bool Import(Student student) // Save students imported from uploaded csv file onto the database
        {
            try
            {
                _appDbContext.student.Add(student);
                _appDbContext.SaveChanges();
                return true;
            }
            catch(DbUpdateException) // If the uploaded data already exists on the database, only the new data is added.
            {
                return false; // False is returned to update the user of existing data having been uploaded
            }
        }
    }
}
