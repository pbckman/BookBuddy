using BookBuddy.Data.Contexts;
using BookBuddy.Data.Entities;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class ProfileController(UserManager<ApplicationUser> userManager, DataContext dataContext) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly DataContext _context = dataContext;

        [Route("/profile/userprofile")]
        public IActionResult UserProfile()
        {
            return View();
        }

        [HttpPost]
        [Route("/profile/userprofile")]
        public async Task<IActionResult> UserProfile(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var profile = new UserProfileEntity
                    {
                        ProfileFirstName = model.Firstname,
                        UserId = user.Id,
                        IsMainProfile = false
                    };

                    _context.Profiles.Add(profile);
                    await _context.SaveChangesAsync();

                    return View(model);
                }
            }

            return View(model);
        }

        
    }
}
