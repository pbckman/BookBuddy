using BookBuddy.Data.Contexts;
using BookBuddy.Data.Entities;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.Controllers
{
    public class AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DataContext dataContext) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly DataContext _context = dataContext;

        [Route("/auth/signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [Route("/auth/signup")]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _userManager.Users.AnyAsync(x => x.Email == model.Email))
                {
                    var user = new ApplicationUser
                    {
                        Email = model.Email,
                        UserName = model.Email,
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var profile = new UserProfileEntity
                        {
                            ProfileFirstName = model.Firstname,
                            ProfileLastName = model.Lastname,
                            IsMainProfile = true,
                            UserId = user.Id
                        };
                        _context.Profiles.Add(profile);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "Startpage");
                    }
                    else
                    {
                        ViewData["StatusMessage"] = "Something went wrong, please try again.";
                    }
                }
                else
                {
                    ViewData["StatusMessage"] = "User with submitted email adress already exists";
                }
            }

            return View(model);
        }

        [Route("/auth/signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("/auth/signin")]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "StartPage");
                    }
                }
            }

            ViewData["StatusMessage"] = "Incorrect email or password";
            return View(model);
        }

        [HttpGet]

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Auth");
        }
    }
}
