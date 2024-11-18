using System;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels;

public class MyQuizzesPageViewModel(MyQuizzesPage currentPage, SiteSettingsPage siteSettings) : PageViewModel<MyQuizzesPage>(currentPage, siteSettings)
{
    
}
