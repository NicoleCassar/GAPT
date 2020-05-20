using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public interface IPreferenceRepository
    {
        public IEnumerable<Preference> GetAllData();
        void Submit(Preference preference);

        Area GetAreaByTitle(string id);


    }
}
