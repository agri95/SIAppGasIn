using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SiappGasIn.Models
{
    public class SysUserProfile
    {
        [Key]
        public int UserProfileId { get; set; }

        [MaxLength(50, ErrorMessage = "FirstName value can not exceed 50 characters. ")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "LastName value can not exceed 50 characters. ")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Email value can not exceed 50 characters. ")]
        public string Email { get; set; }

        [MaxLength(50, ErrorMessage = "JobTitle value can not exceed 50 characters. ")]
        public string JobTitle { get; set; }

        
        [MaxLength(200, ErrorMessage = "ProfilePicture path value cannot exceed 200 characters. ")]
        public string ProfilePicture { get; set; } = "~/adminlte/images/blank-person.png";

        [Required]
        public string ApplicationUserId { get; set; }

        [MaxLength(50, ErrorMessage = "CreatedBy value cannot exceed 50 characters. ")]
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }

        [MaxLength(50, ErrorMessage = "ModifiedBy value cannot exceed 50 characters. ")]
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }

    public class RoleUserList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsChecked { get; set; }

    }
    public class RoleUser
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsChecked { get; set; }

    }

}