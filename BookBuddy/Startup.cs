using BookBuddy.Business.Clients;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.AiService;
using BookBuddy.Business.Services.BookContentService;
using BookBuddy.Business.Services.BookPageService;
using BookBuddy.Business.Services.BookService;
using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.BooksPageService;
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
using BookBuddy.Business.Services.ScheduledJobsService;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.QuizService;
using BookBuddy.Business.Services.QuizResultService;
using BookBuddy.Business.Services.StateService;
using Blazored.LocalStorage;
using BookBuddy.Business.Services.CategoryService;
using BookBuddy.Business.Services.SiteSettingsService;
using Microsoft.Extensions.Logging.Abstractions;
using BookBuddy.Business.Services.LanguageService;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using BookBuddy.Business.Services.StartPageService;
using BookBuddy.Business.Services.ErrorMessageService;
using BookBuddy.Business.Services.SiteMapService;
using Microsoft.AspNetCore.Authentication.Cookies;
using BookBuddy.Business.Services.Middleware;
using Microsoft.AspNetCore.Identity;


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
            services.AddSingleton<ITranslationService>(provider => new TranslationService(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Translations"), provider.GetRequiredService<ILogger<TranslationService>>()));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookContentService, GutenbergService>();
            services.AddScoped<IAiService, OpenAiService>();
            services.AddScoped<IBookPageService, BookPageService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IQuizFactory, QuizFactory>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuizResultService, QuizResultService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<QuizResultFactory>();
            services.AddScoped<OpenAiClient>();
            services.AddScoped<IBooksPageService, BooksPageService>();
            services.AddScoped<IScheduledJobsService, ScheduledJobsService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddTransient<SiteSettingsService>();
            services.AddTransient<BookPageFactory>();
            services.AddScoped<TranslationFactory>();
            services.AddHttpClient();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "PolicyScheme"; 
                        })
            .AddPolicyScheme("PolicyScheme", "Policy Scheme", options =>
            {
                options.ForwardDefaultSelector = context =>
                {

                    if (context.Request.Path.StartsWithSegments("/cms"))
                    {
                        return "CmsIdentityScheme"; 
                    }
                    else
                    {
                        return "PublicIdentityScheme";  
                    }
                };
            })
            .AddCookie("CmsIdentityScheme", options =>
            {
                options.LoginPath = "/cms/auth/signin";
                options.AccessDeniedPath = "/cms/account/accessdenied";
            })
            .AddCookie("PublicIdentityScheme", options =>
            {
                options.LoginPath = "/auth/signin";
                options.AccessDeniedPath = "/account/accessdenied";

                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (!context.Request.Path.StartsWithSegments("/auth/signin"))
                        {
                            var lang = context.Request.Query["lang"].FirstOrDefault() ??
                                       (context.Request.Path.ToString().Contains("/sv/") ? "sv" : "en");

                            // Dynamiskt sätta inloggnings-URL:n
                            var loginUrl = $"/{lang}/auth/signin";
                            context.Response.Redirect(loginUrl);
                        }
                        return Task.CompletedTask;
                    }
                };
            });


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

              
                var culture = context.HttpContext.Request.Path.Value?.Split('/').FirstOrDefault(s => s == "sv" || s == "en") ?? "en";

                if (string.IsNullOrEmpty(culture))
                {
                    culture = "en";
                }
                var redirectUrl = $"/{culture}/error?statusCode={statusCode}";

                response.Redirect(redirectUrl);
                await Task.Yield();

            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<ApprovalMiddleware>();
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
