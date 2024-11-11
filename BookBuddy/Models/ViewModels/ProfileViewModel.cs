using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "You must enter a name")]
        [MinLength(2, ErrorMessage = "A valid name is required")]
        [Display(Name = "Name", Prompt = "Enter your name")]
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
    }
}
