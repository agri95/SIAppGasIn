using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstParameter
    {
        [Key]
        public int ParamId { get; set; }

        [Required]
        [MaxLength(350, ErrorMessage = "ParamName can not exceed 350 characters. ")]
        public string ParamName { get; set; }
        
        [Required]
        public decimal ParamValue { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
