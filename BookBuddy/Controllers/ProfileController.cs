using BookBuddy.Models.ViewModels;
using BookBuddy.Services;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class ProfileController(UserManager<ApplicationUser> userManager, ProfileService profileService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ProfileService _profileService = profileService;

        [HttpGet]
        [Route("/profile/userprofile")]
        public IActionResult UserProfile()
        {
            return View();
        }


        [HttpPost]
        [Route("/profile/userprofile")]
        public async Task<IActionResult> CreateProfile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            await _profileService.CreateProfileAsync(user.Id, model.FirstName, model.LastName, isMainProfile: false);

            return RedirectToAction("UserProfile", "Profile");
        }

        [HttpGet]
        [Route("/profile/updateprofile")]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var profile = await _profileService.GetProfileAsync(user);
            if (profile == null)
            {
                return NotFound();
            }  
            var model = new ProfileViewModel
            {
                FirstName = profile.ProfileFirstName
            };

            return View(model);
        }

        [HttpPost]
        [Route("/profile/updateprofile")]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var success = await _profileService.UpdateProfileAsync(user, model);
            if (!success)
            {
                ViewData["StatusMessage"] = "Could not update profile, please try again later.";
                return View(model);
            }

            return RedirectToAction("UserProfile", "Profile");
        }

        [HttpPost]
        [Route("/profile/deleteprofile/{profileId}")]
        public async Task<IActionResult> DeleteProfile(int profileId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var profile = await _profileService.GetProfileByIdAsync(profileId);
            if (profile == null || profile.UserId != user.Id)
            {
                return NotFound();
            }

            var success = await _profileService.DeleteProfileAsync(profile);
            if (!success)
            {
                ViewData["StatusMessage"] = "Failed to delete profile";
                return View(profile);
            }

            return RedirectToAction("UserProfile", "Profile");
        }


    }
}
