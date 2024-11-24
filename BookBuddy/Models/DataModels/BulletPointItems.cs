using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.DataModels
{
    public class BulletPointItems 
    {
        [Display(Name = "Text")]
        public string? Text {  get; set; }
    }
}
