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
    }
}
