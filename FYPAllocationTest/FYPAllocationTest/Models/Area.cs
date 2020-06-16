using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPAllocationTest.Models
{
    public class Area // This is the model responsible for reading and writing data to and from the 'supervisor_area' table
    {
        [Key]
        public int area_id { get; set; } // Each area is given a unique id
        [ForeignKey("supervisor")]
        public string supervisor_id { get; set; } // Areas are linked to supervisors through the use of 'supervisor_id' as a Foreign Key
        public Supervisor supervisor { get; set; }
        public string title { get; set; } // Title of the given area
        public string description { get; set; } // Description of the area
        public bool available { get; set; } // Setting areas availability to true by default, this value is to be passed to the allocation algorithm
        public int area_quota { get; set; } // Quota for each area
        public string keywords { get; set; } // Any keywords supervisors opted to add to further describe an area
        public string required_resources { get; set; } // Any possible resources that may be required in advance
        public string required_prerequisites { get; set; } // Any study units that students may follow in order to better understand the area
        public string ethical_issues { get; set; } // Possible ethical issues that may be present in the area
        public string area_code { get; set; } // A unique code generated upon allocation, as a means of compressing data into the allocation results table
    }
}
