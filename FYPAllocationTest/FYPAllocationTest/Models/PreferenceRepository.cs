using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly AppDbContext _appDbContext;

        public PreferenceRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Preference> GetAllData()
        {
            return _appDbContext.student_preference;
        }

        public Area GetAreaByTitle(string title)
        {
            return _appDbContext.supervisor_area.FirstOrDefault(p => p.title == title);
        }

        public void Submit(Preference preference)
        {
            
            _appDbContext.student_preference.Add(preference);
            _appDbContext.SaveChanges();
        }
    }
}
