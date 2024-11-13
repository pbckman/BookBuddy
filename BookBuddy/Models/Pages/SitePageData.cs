using System.ComponentModel.DataAnnotations;
using EPiServer.Web;

namespace BookBuddy.Models.Pages
{
    public class SitePageData : PageData
    {
    [Display(
    GroupName = Globals.GroupNames.MetaData,
    Order = 100
    )]
    [CultureSpecific]
    public virtual string MetaTitle
    {
        get
        {
            var metaTitle = this.GetPropertyValue(p => p.MetaTitle);

            return !string.IsNullOrEmpty(metaTitle) ? metaTitle : PageName;
        }
        set => this.SetPropertyValue(p => p.MetaTitle, value);
    }
    
    [Display(
    GroupName = Globals.GroupNames.MetaData,
    Order = 200
    )]
    [CultureSpecific]
    public virtual string MetaDescription
    {
        get {
            var metaDescription = this.GetPropertyValue(p => p.MetaDescription);

            return !string.IsNullOrEmpty(metaDescription) ? metaDescription : PageName;
        }
        set => this.SetPropertyValue(p => p.MetaDescription, value);
    }

    [Display(
    GroupName = Globals.GroupNames.Navbar,
    Order = 300
    )]
    [CultureSpecific]
    [UIHint(UIHint.Image)]
    public virtual ContentReference? NavbarIcon { get; set; }

    [Display(
    GroupName = Globals.GroupNames.Navbar,
    Order = 400
    )]
    [CultureSpecific]
    public virtual string NavbarIconAltText { get; set; } = string.Empty;

    [Display(
    GroupName = Globals.GroupNames.Navbar,
    Order = 500
    )]
    [CultureSpecific]
    public virtual string NavbarTitle { 
        get {
            var navbarTitle = this.GetPropertyValue(p => p.NavbarTitle);

            return !string.IsNullOrEmpty(navbarTitle) ? navbarTitle : PageName;
        }
        set => this.SetPropertyValue(p => p.NavbarTitle, value); 
    }


    }
}
