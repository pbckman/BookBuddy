using System;
using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels;

public class QuizPageViewModel(QuizPage currentPage, SiteSettingsPage siteSettings) : PageViewModel<QuizPage>(currentPage, siteSettings)
{
}
