using BookBuddy.Models.DataModels;

namespace BookBuddy.Business.Services.CategoryService;

public interface ICategoryService
{
    List<CategoryModel> GetAllCategories(string currentCulture);
}
