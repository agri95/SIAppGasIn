using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class HeaderSimulationCost
    {
      
        [Key]
        public int HeaderSimulationID { get; set; }
        public string ProjectName { get; set; }
        public string Creator { get; set; }
        public DateTime? ProjectDate { get; set; }
        public string CustomerName { get; set; }
        public DateTime? TargetCOD { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
