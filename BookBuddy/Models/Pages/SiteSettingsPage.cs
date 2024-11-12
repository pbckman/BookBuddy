using EPiServer.SpecializedProperties;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.Pages
{
    [ContentType(
        DisplayName = "Site Settings page", 
        GUID = "834C413F-F661-4E64-B656-479E6D91D852", 
        Description = "Settings for site-wide elements like header, footer, etc.")]
    public class SiteSettingsPage : PageData
    {
        [CultureSpecific]
        [Display(Name = "Header Logo", GroupName = Globals.GroupNames.Header, Order = 10)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference? HeaderLogo { get; set; }

        [CultureSpecific]
        [Display(Name = "Header Title", GroupName = Globals.GroupNames.Header, Order = 15)]
        public virtual string HeaderTitle { get; set; } = string.Empty;

        [CultureSpecific]
        [Display(Name = "Header Links", GroupName = Globals.GroupNames.Header, Order = 20)]
        public virtual LinkItemCollection? HeaderLinks { get; set; }

        [CultureSpecific]
        [Display(Name = "Social Media Links", GroupName = Globals.GroupNames.Footer, Order = 30)]
        public virtual ContentArea? SocialLinks { get; set; }

        [CultureSpecific]
        [Display(Name = "Footer text", GroupName = Globals.GroupNames.Footer, Order = 40)]
        public virtual string FooterText { get; set; } = string.Empty;
        
        [CultureSpecific]
        [Display(Name = "Navbar Links", GroupName = Globals.GroupNames.Navbar, Order = 50)]
        public virtual LinkItemCollection? NavbarLinks { get; set; }

        [CultureSpecific]
        [Display(Name = "Navbar Logo", GroupName = Globals.GroupNames.Navbar, Order =55)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference? NavbarLogo { get; set; }

        [CultureSpecific]
        [Display(Name = "Navbar Logo Alt Text", GroupName = Globals.GroupNames.Navbar, Order = 60)]
        public virtual string NavbarLogoAltText { get; set; } = string.Empty;

        [CultureSpecific]
        [Display(Name = "Navbar Link List", GroupName = Globals.GroupNames.Navbar, Order = 65)]
        public virtual IList<PageReference>? NavbarLinkList { get; set; }

    }
}
