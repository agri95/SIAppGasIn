using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstEnergy
    {
        [Key]
        public int EnergyID { get; set; }
        public string Energy { get; set; }

        public decimal NilaiKalori { get; set; }
        public string Satuan { get; set; }        

        public string CreatedBy { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
