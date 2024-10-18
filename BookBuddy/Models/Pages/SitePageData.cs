using System.ComponentModel.DataAnnotations;

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
    }
}
