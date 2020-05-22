using Microsoft.EntityFrameworkCore;
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

        public Area GetAreaById(int Id)
        {
            return _appDbContext.supervisor_area.FirstOrDefault(p => p.area_id == Id);
        }

        public Area getNextID()
        {
            var nextid =  _appDbContext.supervisor_area.OrderByDescending(p => p.area_id).First();
            if (nextid != null)
                return nextid;
            else
                return null;
        }

        public IEnumerable<Area> GetAllData()
        {
            return _appDbContext.supervisor_area;
        }

        public void UpdateQuota(Area new_area)
        {
            var current_area = _appDbContext.supervisor_area.SingleOrDefault(a => a.area_id == new_area.area_id);
            current_area.area_quota = new_area.area_quota;
            _appDbContext.Entry(current_area).State = EntityState.Modified;
            _appDbContext.SaveChanges();
        }
    }
}
