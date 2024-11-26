using System;

namespace BookBuddy.Business.Services.AuthorizedService;

public interface IAuthorizedService
{
    Task<bool> IsUserAuthorizedAsync();
}
