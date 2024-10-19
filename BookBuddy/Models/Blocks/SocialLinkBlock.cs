using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.Blocks
{
    [ContentType(DisplayName = "Social Link Block", GUID = "38970E58-978B-4F13-8212-6690A15D4DD9", Description = "Block for Social media link")]
    public class SocialLinkBlock : BlockData
    {
        [Display(Name = "Platform Name", Order = 10)]
        public virtual string PlatformName { get; set; }

        [Display(Name = "Url", Order = 20)]
        public virtual Url Url { get; set; }

        [Display(Name = "Icon", Order = 30)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Icon { get; set; }

    }
}
