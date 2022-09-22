using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstGaji
    {
        [Key]
        public int GajiID { get; set; }
        public int LokasiID { get; set; }
        public int TypeID { get; set; }
        public decimal Gaji { get; set; }
        public int Tahun { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }

    public class MstListGaji
    {
        [Key]
        public int GajiID { get; set; }
        public string NamaSPBG { get; set; }
        public string Profesi { get; set; }
        public decimal Gaji { get; set; }
        public int Tahun { get; set; }
    }
    
    public class SP_GetGajiByLocationName
    {
       
        public string NamaSPBG { get; set; }
        public decimal GajiSupir { get; set; }
        public decimal GajiOpeator { get; set; }
    }
}



