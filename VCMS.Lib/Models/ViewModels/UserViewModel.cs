using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Display(Name = "Lockout")]
        public bool Lockout { get; set; }

        [Display(Name = "Lockout end date")]
        public DateTime? LockoutEndDate { get; set; }

        [Display(Name = "Roles")]
        public string[] Roles { get; set; }
    }
}
