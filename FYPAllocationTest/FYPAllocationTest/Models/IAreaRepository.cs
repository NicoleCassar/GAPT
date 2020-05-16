using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public interface IAreaRepository
    {
        public IEnumerable<Area> GetAllData();
        public void Submit(Area area);
        public Area getNextID();
    }
}
