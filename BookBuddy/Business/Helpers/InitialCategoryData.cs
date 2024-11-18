using System;
using BookBuddy.Models.DDS;

namespace BookBuddy.Business.Helpers;

public class InitialCategoryData
{
    public static List<CategoryItem> CategoryItemsSV => new List<CategoryItem>
    {
        new CategoryItem { Text = "Äventyr", Value = "Aventyr", Language = "sv" },
        new CategoryItem { Text = "Sagor", Value = "Sagor", Language = "sv" },
        new CategoryItem { Text = "Godnattsagor", Value = "Godnattsagor", Language = "sv" },
        new CategoryItem { Text = "Utbildande", Value = "Utbildande", Language = "sv" },
        new CategoryItem { Text = "Bildböcker", Value = "Bildbocker", Language = "sv" },
        new CategoryItem { Text = "Poesi", Value = "Poesi", Language = "sv" },
        new CategoryItem { Text = "Djurberättelser", Value = "Djurberattelser", Language = "sv" },
        new CategoryItem { Text = "Klassisk Litteratur", Value = "KlassiskLitteratur", Language = "sv" },
        new CategoryItem { Text = "Noveller", Value = "Noveller", Language = "sv" },
        new CategoryItem { Text = "Interaktiva Böcker", Value = "InteraktivaBocker", Language = "sv" },
        new CategoryItem { Text = "Biografi", Value = "Biografi", Language = "sv" },
        new CategoryItem { Text = "Fantasy", Value = "Fantasy", Language = "sv" },
        new CategoryItem { Text = "Serier", Value = "Serier", Language = "sv" },
        new CategoryItem { Text = "Familj & Vänner", Value = "FamiljVanner", Language = "sv" },
        new CategoryItem { Text = "Vetenskap & Natur", Value = "VetenskapNatur", Language = "sv" },
        new CategoryItem { Text = "Äventyr & Mysterium", Value = "AventyrMysterium", Language = "sv" },
    };
    public static List<CategoryItem> CategoryItemsEN => new List<CategoryItem>
    {
        new CategoryItem { Text = "Adventure", Value = "Adventure", Language = "en" },
        new CategoryItem { Text = "Fairy Tales", Value = "FairyTales", Language = "en" },
        new CategoryItem { Text = "Bedtime Stories", Value = "BedtimeStories", Language = "en" },
        new CategoryItem { Text = "Educational", Value = "Educational", Language = "en" },
        new CategoryItem { Text = "Picture Books", Value = "PictureBooks", Language = "en" },
        new CategoryItem { Text = "Poetry", Value = "Poetry", Language = "en" },
        new CategoryItem { Text = "Animal Stories", Value = "AnimalStories", Language = "en" },
        new CategoryItem { Text = "Classic Literature", Value = "ClassicLiterature", Language = "en" },
        new CategoryItem { Text = "Short Stories", Value = "ShortStories", Language = "en" },
        new CategoryItem { Text = "Interactive Books", Value = "InteractiveBooks", Language = "en" },
        new CategoryItem { Text = "Biography", Value = "Biography", Language = "en" },
        new CategoryItem { Text = "Fantasy", Value = "Fantasy", Language = "en" },
        new CategoryItem { Text = "Comics", Value = "Comics", Language = "en" },
        new CategoryItem { Text = "Family & Friends", Value = "FamilyFriends", Language = "en" },
        new CategoryItem { Text = "Science & Nature", Value = "ScienceNature", Language = "en" },
        new CategoryItem { Text = "Adventure & Mystery", Value = "AdventureMystery", Language = "en" },
    };
}
