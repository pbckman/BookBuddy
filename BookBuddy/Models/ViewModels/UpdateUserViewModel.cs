using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "You must enter a password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])(?!.*\s).{8,}$", ErrorMessage = "A valid password is required")]
        [Display(Name = "Current password", Prompt = "Enter your current password")]
        public string? CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])(?!.*\s).{8,}$", ErrorMessage = "A valid password is required")]
        [Display(Name = "New password", Prompt = "Enter your new password")]

        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password")]
        [Display(Name = "Confirm password", Prompt = "Confirm your password")]
        [Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmNewPassword { get; set; }

    }
}
