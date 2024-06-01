using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApplication2.ViewModels
{
    public class EditProfileViewModel
    {

        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter your birth date")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

    }
}
