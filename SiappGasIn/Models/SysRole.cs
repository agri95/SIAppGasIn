using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class SysRoleViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; }
        

    }

    
    public class UserRoleList
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string Email { get; set; }
        public string IsChecked { get; set; }

    }

    public class UserRole
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsChecked { get; set; }

    }

}

