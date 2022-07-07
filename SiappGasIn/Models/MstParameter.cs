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
        [MaxLength(50, ErrorMessage = "ParamCode value can not exceed 50 characters. ")]
        public string ParamCode { get; set; }

        [Required]
        [MaxLength(350, ErrorMessage = "ParamValue value can not exceed 350 characters. ")]
        public string ParamValue { get; set; }

        [Required]
        [MaxLength(350, ErrorMessage = "ParamName can not exceed 350 characters. ")]
        public string ParamName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "ParamGroup can not exceed 50 characters. ")]
        public string ParamGroup { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
