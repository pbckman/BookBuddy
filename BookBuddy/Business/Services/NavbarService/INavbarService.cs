using System;
using BookBuddy.Models.DataModels;

namespace BookBuddy.Business.Services.NavbarService;

public interface INavbarService
{
    IEnumerable<NavbarLinkModel> GetNavbarProperties(List<PageReference> pageReferences);
}
