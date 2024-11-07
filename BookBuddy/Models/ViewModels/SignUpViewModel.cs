using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.ViewModels;

public class SignUpViewModel
{
    [Required(ErrorMessage = "You must enter a first name")]
    [MinLength(2, ErrorMessage = "A valid first name is required")]
    [Display(Name = "First name", Prompt = "Enter your first name")]
    public string Firstname { get; set; } = null!;

    [Required(ErrorMessage = "You must enter a last name")]
    [MinLength(2, ErrorMessage = "A valid last name is required")]
    [Display(Name = "Last name", Prompt = "Enter your last name")]
    public string Lastname { get; set; } = null!;

    [Required(ErrorMessage = "You must enter an email address")]
    [Display(Name = "Email address", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "You must enter a password")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])(?!.*\s).{8,}$", ErrorMessage = "A valid password is required")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Password and confirmation password do not match.")]
    [Display(Name = "Confirm password", Prompt = "Confirm your password")]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}
