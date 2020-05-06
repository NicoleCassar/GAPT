using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class Area
    {
        [Key]
        public int area_id { get; set; }
        public string supervisor_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool available { get; set; }
        public int area_quota { get; set; }
    }
}
