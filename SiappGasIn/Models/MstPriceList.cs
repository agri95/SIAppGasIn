using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class MstPriceList
    {
        [Key]
        public int PriceListId { get; set; }

        [Required]
        [MaxLength(350, ErrorMessage = "Description can not exceed 350 characters. ")]
        public string Description { get; set; }
        public string Reference { get; set; }
        public decimal Value { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
