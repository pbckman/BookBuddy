using System;
using System.Globalization;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.DDS;
using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Http.Features;



namespace BookBuddy.Business.Factories;

public class CategorySelectionFactory : ISelectionFactory
{
    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
    {        
        var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        var selectedValues = metadata.Model as IEnumerable<string> ?? new List<string>();

        var allCategories = new List<CategoryItem>
        {
            // Engelska kategorier
            new CategoryItem { Text = "Adventure", Value = "Adventure", Language = "en" },
            new CategoryItem { Text = "Fairy Tales", Value = "Fairy Tales", Language = "en" },
            new CategoryItem { Text = "Bedtime Stories", Value = "Bedtime Stories", Language = "en" },
            new CategoryItem { Text = "Educational", Value = "Educational", Language = "en" },
            new CategoryItem { Text = "Picture Books", Value = "Picture Books", Language = "en" },
            new CategoryItem { Text = "Poetry", Value = "Poetry", Language = "en" },
            new CategoryItem { Text = "Animal Stories", Value = "Animal Stories", Language = "en" },
            new CategoryItem { Text = "Classic Literature", Value = "Classic Literature", Language = "en" },
            new CategoryItem { Text = "Short Stories", Value = "Short Stories", Language = "en" },
            new CategoryItem { Text = "Interactive Books", Value = "Interactive Books", Language = "en" },
            new CategoryItem { Text = "Biography", Value = "Biography", Language = "en" },
            new CategoryItem { Text = "Fantasy", Value = "Fantasy", Language = "en" },
            new CategoryItem { Text = "Comics", Value = "Comics", Language = "en" },
            new CategoryItem { Text = "Family & Friends", Value = "Family & Friends", Language = "en" },
            new CategoryItem { Text = "Science & Nature", Value = "Science & Nature", Language = "en" },
            new CategoryItem { Text = "Adventure & Mystery", Value = "Adventure & Mystery", Language = "en" },

            // Svenska kategorier
            new CategoryItem { Text = "Äventyr", Value = "Äventyr", Language = "sv" },
            new CategoryItem { Text = "Sagor", Value = "Sagor", Language = "sv" },
            new CategoryItem { Text = "Godnattsagor", Value = "Godnattsagor", Language = "sv" },
            new CategoryItem { Text = "Utbildande", Value = "Utbildande", Language = "sv" },
            new CategoryItem { Text = "Bilderböcker", Value = "Bilderböcker", Language = "sv" },
            new CategoryItem { Text = "Poesi", Value = "Poesi", Language = "sv" },
            new CategoryItem { Text = "Djurberättelser", Value = "Djurberättelser", Language = "sv" },
            new CategoryItem { Text = "Klassisk Litteratur", Value = "Klassisk Litteratur", Language = "sv" },
            new CategoryItem { Text = "Noveller", Value = "Noveller", Language = "sv" },
            new CategoryItem { Text = "Interaktiva Böcker", Value = "Interaktiva Böcker", Language = "sv" },
            new CategoryItem { Text = "Biografi", Value = "Biografi", Language = "sv" },
            new CategoryItem { Text = "Fantasy", Value = "Fantasy", Language = "sv" },
            new CategoryItem { Text = "Serier", Value = "Serier", Language = "sv" },
            new CategoryItem { Text = "Familj & Vänner", Value = "Familj & Vänner", Language = "sv" },
            new CategoryItem { Text = "Vetenskap & Natur", Value = "Vetenskap & Natur", Language = "sv" },
            new CategoryItem { Text = "Äventyr & Mysterium", Value = "Äventyr & Mysterium", Language = "sv" },
        };

        var languageFilteredCategories = allCategories
            .Where(c => c.Language == currentCulture)
            .Select(category => new CustomSelectedItem
            {
                Text = category.Text,
                Value = category.Value,
                Selected = selectedValues.Contains(category.Value)
            })
            .ToArray();

        return languageFilteredCategories;

    }
}
