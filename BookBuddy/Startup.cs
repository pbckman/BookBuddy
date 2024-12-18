

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

            services.Configure<FormsConfigOptions>(options =>
            {
                options.InjectFormOwnStylesheet = false;
            });

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
            services.AddTransient<IValidate<AchievementBlock>, AchievementBlockValidator>();
            services.AddScoped<IQuizFactory, QuizFactory>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuizResultService, QuizResultService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<QuizResultFactory>();
            services.AddScoped<OpenAiClient>();
            services.AddScoped<IBooksPageService, BooksPageService>();
            services.AddScoped<IScheduledJobsService, ScheduledJobsService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMyQuizzesService, MyQuizzesService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IAuthorizedService, AuthorizedService>();
            services.AddTransient<SiteSettingsService>();
            services.AddScoped<StartPageService>();
            services.AddTransient<StartPageFactory>();
            services.AddTransient<BookPageFactory>();
            services.AddTransient<CategorySelectionFactory>();
            services.AddTransient<TranslationFactory>();
            services.AddTransient<MyQuizzesFactory>();
            services.AddHttpClient();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();


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

                var path = context.HttpContext.Request.Path.Value;
                var culture = path?.Split('/').FirstOrDefault(s => s == "sv") ?? "en"; 

                var redirectUrl = culture == "en"
                    ? $"/error?statusCode={statusCode}" 
                    : $"/{culture}/error?statusCode={statusCode}";

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
