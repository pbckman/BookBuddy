using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.ViewModels
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "You must enter a name")]
        [MinLength(2, ErrorMessage = "A valid name is required")]
        [Display(Name = "Name", Prompt = "Enter your first name")]

        public string Firstname { get; set; } = null!;

        [Required(ErrorMessage = "You must enter a name")]
        [MinLength(2, ErrorMessage = "A valid name is required")]
        [Display(Name = "Name", Prompt = "Enter your last name")]
        public string Lastname { get; set; } = null!;
    }
}
