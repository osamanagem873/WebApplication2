using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApplication2.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your current password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter your new password")]

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
