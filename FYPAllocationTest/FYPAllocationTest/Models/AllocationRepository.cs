using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class AllocationRepository : IAllocationRepository
    {
        private readonly AppDbContext _appDbContext;

        public AllocationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Allocation> GetAllData()
        {
            return _appDbContext.allocation;
        }

        public void Create(Allocation allocation)
        { 
            _appDbContext.allocation.Add(allocation);
            _appDbContext.SaveChanges();
        }

        public void Delete()
        {
            _appDbContext.allocation.RemoveRange(_appDbContext.allocation);
        }
    }
}
