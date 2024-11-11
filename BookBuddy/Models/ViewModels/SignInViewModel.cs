using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        [Display(Name = "Email", Prompt = "Enter your email address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Password", Prompt = "Enter your password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])(?!.*\s).{8,}$", ErrorMessage = "A valid password is required")]
        public string Password { get; set; } = null!;

        [Display(Name = "Keep me signed in")]
        public bool RememberMe { get; set; }
    }
}
