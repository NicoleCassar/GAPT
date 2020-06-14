using System.Collections.Generic;

namespace FYPAllocationTest.Models
{
    public class AllocationRepository : IAllocationRepository // This repository deals with communication made with the allocations table within the database
    {
        private readonly AppDbContext _appDbContext; // Reference to DbContext

        public AllocationRepository(AppDbContext appDbContext) // Constructor for AllocationRepository
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Allocation> GetAllData() // Rerieves all allocation data from the database
        {
            return _appDbContext.allocation;
        }

        public void Create(Allocation allocation) // Adds a newly created allocation to the database
        { 
            _appDbContext.allocation.Add(allocation);
            _appDbContext.SaveChanges();
        }

        public void Delete() // Deletes all allocations from the database
        {
            _appDbContext.allocation.RemoveRange(_appDbContext.allocation);
        }
    }
}
