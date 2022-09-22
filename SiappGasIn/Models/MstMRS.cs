using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstMRS
    {
        [Key]
        public int MRSID { get; set; }
        public string ClassID { get; set; }
        public string TypePelanggan { get; set; }
        public decimal MaxPress { get; set; }
        public decimal MinPress { get; set; }
        public decimal InletMaxBarg { get; set; }
        public decimal InletMinBarg { get; set; }
        public decimal OutletMaxBarg { get; set; }
        public decimal OutletMinBarg { get; set; }
        public decimal InletInch { get; set; }
        public decimal OutletInch { get; set; }
        public decimal InletNPS { get; set; }
        public decimal OutletNPS { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }


}

