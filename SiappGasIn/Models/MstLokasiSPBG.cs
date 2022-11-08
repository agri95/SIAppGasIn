using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstLokasiSPBG
    {
        [Key]
        public int LokasiID { get; set; }
        public string NamaSPBG { get; set; }
        public string LokasiSPBG { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public decimal HargaUS { get; set; }
        public decimal HargaIDR { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }

    public class SP_GetStation
    {
        public int LokasiID { get; set; }
        public string NamaSPBG { get; set; }
        public string LokasiSPBG { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public decimal HargaUS { get; set; }
        public decimal HargaIDR { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string TypeModa { get; set; }
    }
}


