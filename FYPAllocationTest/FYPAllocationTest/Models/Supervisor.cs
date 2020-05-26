using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class Supervisor
    {
        [Key]
        public string supervisor_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public int quota { get; set; }

    }
}
//623222150131-ubbqhqigbis91aj9rsbpmkv1cguc1422.apps.googleusercontent.com
//HKbxfpASOPACZg1BbzQQuGcn