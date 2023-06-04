using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
