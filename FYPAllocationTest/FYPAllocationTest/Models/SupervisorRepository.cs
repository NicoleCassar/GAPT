using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class SupervisorRepository : ISupervisorRepository
    {
        private readonly AppDbContext _appDbContext;

        public SupervisorRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Supervisor> GetAllData()
        {
            return _appDbContext.supervisor;
        }
        public bool Import(Supervisor supervisor)
        {
            try
            {
                _appDbContext.supervisor.Add(supervisor);
                _appDbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }


        }
    }
}
