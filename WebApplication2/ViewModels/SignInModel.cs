using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class SignInModel
    {
        
        [Required(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        
    }
}
