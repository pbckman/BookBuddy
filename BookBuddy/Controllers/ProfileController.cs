using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class ProfileController(UserManager<ApplicationUser> userManager, ProfileService profileService, AuthTranslationService translationService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ProfileService _profileService = profileService;
        private readonly AuthTranslationService _translationService = translationService;


        [HttpGet]
        [Route("{lang}/profile/userprofile")]
        public IActionResult UserProfile(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("userprofile", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("userprofile", "description", lang);
            ViewData["FirstName"] = _translationService.GetTranslation("userprofile", "firstname", lang);
            ViewData["SaveButton"] = _translationService.GetTranslation("userprofile", "saveButton", lang);

            return View();
        }


        [HttpPost]
        [Route("{lang}/profile/userprofile")]
        public async Task<IActionResult> CreateProfile(ProfileViewModel model, string lang = "en")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            await _profileService.CreateProfileAsync(user.Id, model.FirstName, model.LastName, isMainProfile: false);

            return RedirectToAction("UserProfile", "Profile");
        }

        [HttpPost]
        [Route("/profile/selectprofile")]
        public IActionResult SelectProfile(int profileId)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append("SelectedProfileId", profileId.ToString(), cookieOptions);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        [Route("{lang}/profile/updateprofile")]
        public async Task<IActionResult> UpdateProfile(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("updateprofile", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("updateprofile", "description", lang);
            ViewData["FirstName"] = _translationService.GetTranslation("updateprofile", "firstname", lang);
            ViewData["SaveButton"] = _translationService.GetTranslation("updateprofile", "saveButton", lang);


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
            var model = new UserProfileViewModel
            {
                Firstname = profile.ProfileFirstName,
                Lastname = profile.ProfileLastName
            };

            return View(model);
        }

        [HttpGet]
        [Route("{lang}/profile/details")]
        public async Task<IActionResult> Details(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("updateprofile", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("updateprofile", "description", lang);
            ViewData["FirstName"] = _translationService.GetTranslation("updateprofile", "firstname", lang);
            ViewData["SaveButton"] = _translationService.GetTranslation("updateprofile", "saveButton", lang);


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
            var model = new UserProfileViewModel
            {
                Firstname = profile.ProfileFirstName,
                Lastname = profile.ProfileLastName
            };

            return View(model);
        }

        [HttpPost]
        [Route("{lang}/profile/details")]
        public async Task<IActionResult> Details(UserProfileViewModel model, string lang = "en")
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
                TempData["StatusMessage"] = "Could not update profile, please try again later.";
                return View(model);
            }

            TempData["StatusMessage"] = "Profile updated successfully!";
            return RedirectToAction("Details", "Profile");
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
