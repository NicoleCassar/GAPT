using System.Collections.Generic;
using System.Linq;

namespace FYPAllocationTest.Models
{
    public class PreferenceRepository : IPreferenceRepository // Repository containing methods for communicating with the 'student_preference' table
    {
        private readonly AppDbContext _appDbContext;

        public PreferenceRepository(AppDbContext appDbContext) // COnstructor for repository
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Preference> GetAllData() // Get all data from 'student_preference' table
        {
            return _appDbContext.student_preference;
        }

        public void Submit(Preference preference) // Add new preference to the 'student_preference' table
        {
            _appDbContext.student_preference.Add(preference);
            _appDbContext.SaveChanges();
        }
    }
}
