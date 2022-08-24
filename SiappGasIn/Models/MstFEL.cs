using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstFEL
    {
        [Key]
        public int FelID { get; set; }
        public int KlasifikasiID { get; set; }
        public int ItemKlasifikasiID { get; set; }
        public int Diameter { get; set; }
        public int UnitID { get; set; }
        public decimal Material300A { get; set; }
        public decimal Material300B { get; set; }
        public decimal Material150A { get; set; }
        public decimal Material150B { get; set; }
        public decimal KontruksiA { get; set; }
        public decimal KontruksiB { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }   
}

