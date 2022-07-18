using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstHargaPRS
    {
        [Key]
        public int HargaPRSID { get; set; }
        public string Flowrate { get; set; }
        public string PRS { get; set; }
        public decimal Harga { get; set; }
        public decimal HargaSewa { get; set; }
        public decimal BiayaMobDemod { get; set; }
        public decimal GasMonitoring { get; set; }        
        public string CreatedBy { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

