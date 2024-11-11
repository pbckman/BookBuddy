using Microsoft.AspNetCore.Identity;

namespace BookBuddy.Data.Entities
{
    public class UserProfileEntity
    {
        public int Id { get; set; }
        public string? ProfileFirstName { get; set; }

        public string? ProfileLastName { get; set; }

        public string? ProfileImage { get; set; }
        public bool IsMainProfile { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}
