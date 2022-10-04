using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class HeaderCluster
    {

        [Key]
        public int HeaderClusterID { get; set; }      
        public string ClusterName { get; set; }
        public int CountCapel { get; set; }
        public decimal TotalVolume { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public HeaderCluster() { }
    }
}
