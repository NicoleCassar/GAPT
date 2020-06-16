using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FYPAllocationTest.Models
{
    public class SupervisorRepository : ISupervisorRepository // Repository holdeing methods that communicate with the 'supervisor' table on the database
    {
        private readonly AppDbContext _appDbContext;
        public SupervisorRepository(AppDbContext appDbContext) // Constructor for repository
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Supervisor> GetAllData() // Get all supervisor data from the database
        {
            return _appDbContext.supervisor;
        }
        public Supervisor GetSupervisorById(string Id) // Get specific supervisor by id for a particular supervisor
        {
            return _appDbContext.supervisor.FirstOrDefault(p => p.supervisor_id == Id);
        }
        public bool Import(Supervisor supervisor) // Save supervisor data imported from a csv file
        {
            try
            {
                _appDbContext.supervisor.Add(supervisor);
                _appDbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException) // If data that already exists is uploaded, inform user of existing data
            {
                return false;
            }
        }
        public void UpdateQuota(Supervisor new_supervisor) // Update total quota for supervisors after allocation is performed
        {
            var current_supervisor = _appDbContext.supervisor.SingleOrDefault(s => s.supervisor_id == new_supervisor.supervisor_id);
            current_supervisor.quota = new_supervisor.quota;
            _appDbContext.Entry(current_supervisor).State = EntityState.Modified;
            _appDbContext.SaveChanges();
        }
    }
}
