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

        public Area GetAreaById(string id)
        {
            return _appDbContext.supervisor_area.FirstOrDefault(p => p.supervisor_id == id);
        }

        public void Submit(Preference preference)
        {
            
            _appDbContext.student_preference.Add(preference);
            _appDbContext.SaveChanges();
        }
    }
}
