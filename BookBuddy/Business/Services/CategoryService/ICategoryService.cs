using System.Globalization;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Services.CategoryService;

public interface ICategoryService
{
    List<CategoryModel> GetAllCategories(CultureInfo currentCulture);
    Task<List<CategoryModel>> GetAllUsedCategories(string query, CultureInfo currentCulture, BooksPage currentPage);
}