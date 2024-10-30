using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.ViewModels
{
    public class ChangePassWordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string? NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}