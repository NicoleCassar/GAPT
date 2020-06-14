using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FYPAllocationTest.Models
{
    public class AreaRepository : IAreaRepository // This repository holds all methods that are used to communicate with the 'supervisor_area' table
    {
        private readonly AppDbContext _appDbContext;
        public AreaRepository(AppDbContext appDbContext) // Constructor for repository
        {
            _appDbContext = appDbContext;
        }

        public void Submit(Area area) // Add new area to the 'supervisor_area' table
        {
            _appDbContext.supervisor_area.Add(area);
            _appDbContext.SaveChanges();
        }

        public Area GetAreaById(int Id) // Retrieve a specific area by 'area_id'
        {
            return _appDbContext.supervisor_area.FirstOrDefault(p => p.area_id == Id);
        }

        public Area GetAreaByTitle(string title) // Get area by title of a given area, used when submitting student preferences
        {
            return _appDbContext.supervisor_area.FirstOrDefault(p => p.title == title);
        }

        public IEnumerable<Area> GetAllData() // Get all data found within the 'supervisor_area' table
        {
            return _appDbContext.supervisor_area;
        }

        public void UpdateQuota(Area new_area) // Update area quota for a specific area following performance of allocation
        {
            var current_area = _appDbContext.supervisor_area.SingleOrDefault(a => a.area_id == new_area.area_id);
            current_area.area_quota = new_area.area_quota;
            _appDbContext.Entry(current_area).State = EntityState.Modified;
            _appDbContext.SaveChanges();
        }

        public void Delete(int id) // Delete a specific area
        {
            var area  = GetAreaById(id);
            _appDbContext.supervisor_area.Remove(area);
            _appDbContext.SaveChanges();
        }

        public void AddAreaCodes(Area new_area) // Add area codes to each area upon performance of allocations
        {
            var current_area = _appDbContext.supervisor_area.SingleOrDefault(a => a.area_id == new_area.area_id);
            current_area.area_code = new_area.area_code;
            _appDbContext.Entry(current_area).State = EntityState.Modified;
            _appDbContext.SaveChanges();
        }

        


    }
}
