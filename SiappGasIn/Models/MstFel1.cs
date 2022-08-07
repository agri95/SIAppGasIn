using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstFel1
    {
        [Key]
        public int FelID { get; set; }
        public int KlasifikasiID { get; set; }
        public int ItemKlasifikasiID { get; set; }
        public int Diameter { get; set; }
        public int UnitID { get; set; }
        public decimal Material300 { get; set; }
        public decimal Material150 { get; set; }
        public decimal Kontruksi { get; set; }
        public decimal Total300 { get; set; }
        public decimal Total150 { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }   
}

