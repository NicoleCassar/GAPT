using System.Collections.Generic;

namespace FYPAllocationTest.Models
{
    public interface IPreferenceRepository // This is an interface used to connect the 'PreferenceRepository' to a controller class
    { // Methods will be discussed within the repository class for preferences
        public IEnumerable<Preference> GetAllData();
        void Submit(Preference preference);
    }
}
