using System.ComponentModel.DataAnnotations;

namespace SiappGasIn.Models
{
    public class UserProfileViewModel
    {
        public string? Id { get; set; }

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Firstname { get; set; }

        public string Lastname { get; set; }
        public string JobTitle { get; set; }

        public bool? IsLockedOut { get; set; }

        public String ProfilePicture { get; set; } = "~/upload/images/blank-person.png";

        public string? FullName
        {
            get { return Firstname + " " + Lastname; }
        }

        public IFormFile? Image { get; set; }
        public string RoleID { get; set; }
    }
}
