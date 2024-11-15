using System;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.DDS;
using EPiServer.Data.Dynamic;

namespace BookBuddy.Business.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ILogger<CategoryService> logger)
    {
        _logger = logger;
    }

    public List<CategoryModel> GetAllCategories(string currentCulture)
    {
        try
        {
            var store = DynamicDataStoreFactory.Instance.GetStore(typeof(CategoryItem));
            var allCategories = store.Items<CategoryItem>().Where(category => category.Language == currentCulture).ToList();
            if (allCategories.Count > 0)
            {
                return allCategories.Select(category => new CategoryModel
                {
                    Text = category.Text,
                    Value = category.Value
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : CategoryService.GetAllCategories() : {ex.Message}");
        }
        return [];
    }
}
