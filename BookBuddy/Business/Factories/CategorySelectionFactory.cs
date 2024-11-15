using System;
using System.Globalization;
using BookBuddy.Business.Services.CategoryService;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.DDS;
using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Http.Features;



namespace BookBuddy.Business.Factories;

public class CategorySelectionFactory : ISelectionFactory
{
    private readonly ICategoryService _categoryService;

    public CategorySelectionFactory(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
    {        
        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var selectedValues = metadata.Model as IEnumerable<string> ?? new List<string>();

        var allCategories = _categoryService.GetAllCategories(currentCulture);

        var languageFilteredCategories = allCategories
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
