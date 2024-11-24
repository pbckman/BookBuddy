using BookBuddy.Business.Factories;
using BookBuddy.Models.Validations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.Blocks;

[ContentType(
 GUID = "9B1B8D80-8037-4191-B40B-7EF5B907B162",
 DisplayName = "Achievement Block",
 Description = "Block for a single achievement"
)]

public class AchievementBlock : BlockData
{
    [Display(
         Name = "Title",
         Description = "Title of the achievement",
         GroupName = SystemTabNames.Content,
         Order = 5)]
    [CultureSpecific]
    public virtual string Title { get; set; }

    [Display(
        Name = "Number of Books",
        Description = "Select the number of books",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    [SelectOne(SelectionFactoryType = typeof(NumberOfBooksSelectionFactory))]
    public virtual int NumberOfBooks { get; set; }

    [Display(
        Name = "Number of Chapters",
        Description = "Select the number of chapters",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    [SelectOne(SelectionFactoryType = typeof(NumberOfChaptersSelectionFactory))]
    public virtual int NumberOfChapters { get; set; }

    [Display(
        Name = "Percentage",
        Description = "Select the percentage",
        GroupName = SystemTabNames.Content,
        Order = 30)]
    [SelectOne(SelectionFactoryType = typeof(PercentageSelectionFactory))]
    public virtual int Percentage { get; set; }

    [Display(
        Name = "Image",
        Description = "Upload an image",
        GroupName = SystemTabNames.Content,
        Order = 40)]
    public virtual ContentReference Image { get; set; }

    [Display(
        Name = "Image Alt Text",
        Description = "Add an image alt text",
        GroupName = SystemTabNames.Content,
        Order = 50)]
    [CultureSpecific]
    public virtual string ImageAltText { get; set; }

  
}


