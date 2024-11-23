using BookBuddy.Models.DataModels;
using BookBuddy.Shared;
using EPiServer.Core;
using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages
{
    [ContentType(
    GUID = "F7A0B723-F6D5-498D-AAAB-69B7C69D19A2",
    GroupName = GroupNames.Specialized
    )]
    [AvailableContentTypes(Availability.Specific, Include =
    [
        typeof(ErrorPage),
        typeof(XmlSitemap),
        typeof(BooksPage),
        typeof(MyQuizzesPage),
        typeof(BooksPage),
        typeof(AvailableBooksPage),
        typeof(ExamplePage),
        typeof(CookieInfoPage),
        typeof(SiteSettingsPage)

        typeof(BooksPage),
        typeof(SlideShowPage),
    ])]

    public class StartPage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 10
        )]
        [CultureSpecific]
        public virtual string Heading { get; set; } = string.Empty;

        [Display(
           GroupName = SystemTabNames.Content,
           Order = 20
        )]
        [CultureSpecific]
        public virtual XhtmlString? MainBody { get; set; }

        [Display(
           GroupName = SystemTabNames.Content,
           Order = 30
        )]
        [CultureSpecific]
        public virtual ContentReference? SiteSettingsPage { get; set; }

        [CultureSpecific]
        [Display(
         Name = "Title for herosection",
         GroupName = "HeroSection",
         Order = 35)]
        public virtual string? Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Description for herosection",
            GroupName = "HeroSection",
            Order = 40)]
        public virtual string? Description { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Button Sign In Text",
            GroupName = "HeroSection",
            Order = 50)]
        public virtual string? ButtonSignIn { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Button Sign Up Text",
            GroupName = "HeroSection",
            Order = 60)]
        public virtual string? ButtonSignUp { get; set; }
      
        [CultureSpecific]
        [Display(
            Name = "InfoSection Title",
            GroupName = "InfoSection",
            Order = 70)]
        public virtual string? InfoTitle { get; set; }

        [CultureSpecific]
        [Display(
            Name = "InfoSection Footer",
            GroupName = "InfoSection",
            Order = 75)]
        public virtual string? InfoFooter { get; set;}

        [CultureSpecific]
        [Display(
            Name = "Bullet Points", 
            GroupName = "InfoSection", 
            Order = 80)]
        public virtual IList<string> BulletPoints { get; set; } = new List<string>();

        [CultureSpecific]
        [Display(
            Name ="Slideshow title",
            GroupName ="Slideshow",
            Order = 85)]
        public virtual string? SlideShowTitle { get; set; }

        [CultureSpecific]
        [Display
            (Name = "Slideshow Content", 
            GroupName = "Slideshow",
            Order = 90)]
        public virtual string? SlideShowContent { get; set; }

    }
}
