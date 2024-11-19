using BookBuddy.Business.Clients;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services;
using BookBuddy.Business.Services.AiService;
using BookBuddy.Business.Services.BookContentService;
using BookBuddy.Business.Services.BookPageService;
using BookBuddy.Business.Services.BookService;
using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Business.Services.Interfaces;
using BookBuddy.Business.Services.PageService;
using BookBuddy.Data.Contexts;
using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Find.Cms;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using BookBuddy.Business.Services.TranslationService;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace BookBuddy
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostingEnvironment;

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
        {
            _webHostingEnvironment = webHostingEnvironment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_webHostingEnvironment.IsDevelopment())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

                services.Configure<SchedulerOptions>(options => options.Enabled = false);
            }
            services.AddControllersWithViews();
            services.AddScoped<AccountService>();
            services.AddScoped<ProfileService>();
            services
                .AddCmsAspNetIdentity<ApplicationUser>()
                .AddCms()
                .AddFind()
                .AddFindCms()
                .AddAdminUserRegistration()
                .AddEmbeddedLocalization<Startup>();

            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("EPiServerDB")));

            services.AddHttpContextAccessor();
            services.AddScoped<IXmlSitemapService, XmlSitemapService>();
            services.AddSingleton<ErrorMessageService>();
            services.AddSingleton<AuthTranslationService>(new AuthTranslationService(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Translations", "Auth.xml")));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookContentService, GutenbergService>();
            services.AddScoped<IAiService, OpenAiService>();
            services.AddScoped<IBookPageService, BookPageService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<OpenAiClient>();
            services.AddScoped<IBooksPageService, BooksPageService>();
            services.AddTransient<BookPageFactory>();
            services.AddHttpClient();
            services.AddServerSideBlazor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                var statusCode = response.StatusCode;

                // H�mta den aktuella kulturen fr�n URL-segment eller anv�nd default "en" om inget segment finns
                var culture = context.HttpContext.Request.Path.Value?.Split('/').FirstOrDefault(s => s == "sv" || s == "en") ?? "en";

                // Om ingen kultur hittas i URL:en, anv�nd standardkulturen (engelska)
                if (string.IsNullOrEmpty(culture))
                {
                    culture = "en";
                }

                // Bygg om URL:en med r�tt spr�ksegment och felkod
                var redirectUrl = $"/{culture}/error?statusCode={statusCode}";

                response.Redirect(redirectUrl);
                await Task.Yield();

            });




            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "localized",
                    pattern: "{lang=en}/{controller=StartPage}/{action=Index}/{id?}");

                endpoints.MapContent();

                endpoints.MapBlazorHub();
            });
        }
    }
}
