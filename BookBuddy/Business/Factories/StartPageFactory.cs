using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Factories
{
    public class StartPageFactory
    {
        public HeroSectionModel Create(HeroSectionModel model)
        {
            return new HeroSectionModel
            {
                Title = model.Title,
                Description = model.Description,
                ButtonSignIn = model.ButtonSignIn,
                ButtonSignUp = model.ButtonSignUp,
            };
        }
        public HeroSectionModel CreateHeroSection(StartPage currentPage)
        {
            return new HeroSectionModel
            {
                Title = currentPage.Title,
                Description = currentPage.Description,
                ButtonSignIn = currentPage.ButtonSignIn,
                ButtonSignUp = currentPage.ButtonSignUp,
            };
        }
        public InfoSectionModel CreateInfoSection(StartPage currentPage)
        {
            return new InfoSectionModel
            {
                InfoTitle = currentPage.InfoTitle,
                InfoFooter = currentPage.InfoFooter,
                BulletPoints = currentPage.BulletPoints.Select(bullet => new BulletPointItems { Text = bullet }).ToList(),
            };
        }
       
    }
}
