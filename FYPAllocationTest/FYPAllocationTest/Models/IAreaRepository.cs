using System.Collections.Generic;

namespace FYPAllocationTest.Models
{
    public interface IAreaRepository // Interface connecting AreaRepository to controller class
    { // Methods to be explained within the 'AreaRepository' class
        public IEnumerable<Area> GetAllData();
        public void Submit(Area area);
        public Area GetAreaById(int id);
        public Area GetAreaByTitle(string id);
        public void UpdateArea(Area new_area);
        public void UpdateQuota(Area area);
        public void Delete(int id);
        public void AddAreaCodes(Area area);
    }
}
