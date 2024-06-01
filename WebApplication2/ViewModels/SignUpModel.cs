using System.ComponentModel.DataAnnotations;

public class PhoneNumberAttribute : RegularExpressionAttribute
{
    public PhoneNumberAttribute() : base(@"^\+\d{1,3}\s\d{3}\s\d{3}\s\d{2}\s\d{2}$")
    {
        ErrorMessage = "Please enter a valid phone number in format +XXX XXX XXX XX XX";
    }
}

public class SignUpModel
{
    [Required(ErrorMessage = "Please enter your First Name")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Please enter your Last Name")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter your Birth Date")]
    [Display(Name = "Birth Date")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "Please enter your phone number")]
    [Display(Name = "Phone Number")]
    [PhoneNumber(ErrorMessage = "Please enter a valid phone number in format +XXX XXX XXX XX XX")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Please enter your address")]
    [Display(Name = "Address")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Please enter your Email")]
    [Display(Name = "Email Address")]
    [EmailAddress(ErrorMessage = "Please enter a valid email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please Enter your password")]
    [Compare("ConfirmPassword", ErrorMessage = "Password doesn't match")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please confirm your password")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}