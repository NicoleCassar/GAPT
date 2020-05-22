using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public interface ISupervisorRepository
    {
        public IEnumerable<Supervisor> GetAllData();
        bool Import(Supervisor supervisor);
        public Supervisor GetSupervisorById(string id);
        void UpdateQuota(Supervisor supervisor);
    }
}
