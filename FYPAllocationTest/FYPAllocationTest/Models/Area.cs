using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class Area
    {
        [Key]
        public int area_id { get; set; }
        [ForeignKey("supervisor")]
        public string supervisor_id { get; set; }
        public Supervisor supervisor { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool available { get; set; }
        public int area_quota { get; set; }
        public string keywords { get; set; }
        public string required_resources { get; set; }
        public string required_prerequisites { get; set; }
        public string ethical_issues { get; set; }
        public string area_code { get; set; }
    }
}
