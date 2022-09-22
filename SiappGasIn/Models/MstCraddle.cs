using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstCraddle
    {
       [Key]
        public int CraddleID { get; set; }
        public string Flowrate { get; set; }
        public string UkuranGTM { get; set; }
        public decimal HargaBeli { get; set; }
        public decimal HargaSewa { get; set; }
        public int Tahun { get; set; }
        public int FillingTime { get; set; }
        public int WaitingTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}


