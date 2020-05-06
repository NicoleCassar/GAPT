using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class AreaRepository : IAreaRepository
    {
        private readonly AppDbContext _appDbContext;

        public AreaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Area> GetAllData()
        {
            return _appDbContext.supervisor_area;
        }
    }
}
