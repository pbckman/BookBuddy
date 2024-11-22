using BookBuddy.Data.Entities;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BookBuddy.Business.Services.AccountService;

public class AccountService(UserManager<ApplicationUser> userManager, ProfileService profileService)
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ProfileService _profileService = profileService;

    public async Task<(IdentityResult, UserProfileEntity?)> CreateUserAsync(SignUpViewModel model)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email == model.Email))
        {
            return (IdentityResult.Failed(new IdentityError { Description = "User with submitted email address already exists" }), null);
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return (result, null);
        }

        var profile = await _profileService.CreateProfileAsync(user.Id, model.Firstname, model.Lastname, isMainProfile: true);
        return (result, profile);
    }

    public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string? currentPassword, string? newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, currentPassword!, newPassword!);
    }

    public async Task<List<ApplicationUser>> GetFrontEndUsersAsync()
    {
        var users = await _userManager.Users
            .Where(u => !u.IsApproved)
            .ToListAsync();

        return users;
    }

    public async Task<ApplicationUser> GetCurrentFrontEndUser(ClaimsPrincipal userPrincipal)
    {
        var frontEndUsers = await GetFrontEndUsersAsync();

        foreach (var frontEndUser in frontEndUsers)
        {
            var userFromPrincipal = await _userManager.GetUserAsync(userPrincipal);

            if (userFromPrincipal != null && userFromPrincipal.Id == frontEndUser.Id)
            {
                return frontEndUser;
            }
        }

        return null;
    }
}
