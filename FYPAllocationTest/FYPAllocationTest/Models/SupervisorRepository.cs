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
        public Supervisor GetSupervisorById(string Id)
        {
                return _appDbContext.supervisor.FirstOrDefault(p => p.supervisor_id == Id);
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

        public void UpdateQuota(Supervisor new_supervisor)
        {
            var current_supervisor = _appDbContext.supervisor.SingleOrDefault(s => s.supervisor_id == new_supervisor.supervisor_id);
            current_supervisor.quota = new_supervisor.quota;
            _appDbContext.Entry(current_supervisor).State = EntityState.Modified;
            _appDbContext.SaveChanges();
        }
    }
}
