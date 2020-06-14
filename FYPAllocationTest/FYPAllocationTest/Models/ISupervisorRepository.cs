using System.Collections.Generic;

namespace FYPAllocationTest.Models
{
    public interface ISupervisorRepository // Interface connection SupervisorRepository to controller class
    { // Methods used are explained within the SupervisorRepository class
        public IEnumerable<Supervisor> GetAllData();
        bool Import(Supervisor supervisor);
        public Supervisor GetSupervisorById(string id);
        void UpdateQuota(Supervisor supervisor);
    }
}
