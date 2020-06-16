using System.Collections.Generic;

namespace FYPAllocationTest.Models
{
    public interface IAllocationRepository // Interface to connect AllocationRepository to controller class
    { // Methods will be discussed within repository class 'AllocationRepository'
        public IEnumerable<Allocation> GetAllData();
        void Create(Allocation allocation);
        void Delete();
    }
}
