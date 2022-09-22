using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstHeadTruck
    {
        [Key]
        public int HeadTruckID { get; set; }
        public string GTM { get; set; }
        public string Ritase { get; set; }
        public decimal HargaSewa { get; set; }
        public decimal RasioBBM { get; set; }
        public int Kecepatan { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}


