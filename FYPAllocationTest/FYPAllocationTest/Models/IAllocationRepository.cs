using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public interface IAllocationRepository
    {
        public IEnumerable<Allocation> GetAllData();

        void Create(Allocation allocation);
    }
}
