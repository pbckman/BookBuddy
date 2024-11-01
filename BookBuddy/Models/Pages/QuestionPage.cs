using static BookBuddy.Globals;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.Pages
{
    [ContentType(
 GroupName = GroupNames.Specialized,
 GUID = "E268788E-4CB0-4613-BCED-B579C23463B0"
)]
    [ImageUrl("/pages/CMS-icon-page-02.png")]
    public class QuestionPage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 10,
            Name = "Question"
        )]
        [CultureSpecific]
        public virtual string Question { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 20,
            Name = "Answer A Text"
        )]
        [CultureSpecific]
        public virtual string AnswerAText { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 30,
            Name = "Answer A Value"
        )]
        [CultureSpecific]
        [Editable(false)]
        public virtual string AnswerAValue { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 40,
            Name = "Answer B Text"
        )]
        [CultureSpecific]
        public virtual string AnswerBText { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 50,
            Name = "Answer B Value"
        )]
        [CultureSpecific]
        [Editable(false)]
        public virtual string AnswerBValue { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 60,
            Name = "Answer C Text"
        )]
        [CultureSpecific]
        public virtual string AnswerCText { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 70,
            Name = "Answer C Value"
        )]
        [CultureSpecific]
        [Editable(false)]
        public virtual string AnswerCValue { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 80,
            Name = "Answer D Text"
        )]
        [CultureSpecific]
        public virtual string AnswerDText { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 90,
            Name = "Answer D Value"
        )]
        [CultureSpecific]
        [Editable(false)]
        public virtual string AnswerDValue { get; set; } = string.Empty;

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 100,
            Name = "Correct Answer"
        )]
        [CultureSpecific]
        [Editable(false)]
        public virtual string CorrectAnswer { get; set; } = string.Empty;
    }
}
