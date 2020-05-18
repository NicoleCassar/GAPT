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

        public void Submit(Area area)
        {

            _appDbContext.supervisor_area.Add(area);
            _appDbContext.SaveChanges();
        }

        public Area getNextID()
        {
            return _appDbContext.supervisor_area.OrderByDescending(p => p.area_id).First();
        }

        public IEnumerable<Area> GetAllData()
        {
            return _appDbContext.supervisor_area;
        }
    }
}
