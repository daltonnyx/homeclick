using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCMS.Lib.Resources;

namespace VCMS.Lib.Models
{
    public class UserViewModel : BaseViewModel
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "The user name address is required")]
        public string UserName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(32, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Strings))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "LockoutEnabled", ResourceType = typeof(Strings))]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Lockout end date")]
        public DateTime? LockoutEndDate { get; set; }

        [Display(Name = "Roles")]
        public string[] RoleList { get; set; }
    }
}
