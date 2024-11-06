using BookBuddy.Business.Clients;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services;
using BookBuddy.Business.Services.AiService;
using BookBuddy.Business.Services.BookContentService;
using BookBuddy.Business.Services.BookPageService;
using BookBuddy.Business.Services.BookService;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Business.Services.Interfaces;
using BookBuddy.Business.Services.PageService;
using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Find.Cms;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;


namespace BookBuddy
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostingEnvironment;

        public Startup(IWebHostEnvironment webHostingEnvironment)
        {
            _webHostingEnvironment = webHostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_webHostingEnvironment.IsDevelopment())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

                services.Configure<SchedulerOptions>(options => options.Enabled = false);
   
            }

            services
                .AddCmsAspNetIdentity<ApplicationUser>()
                .AddCms()
                .AddFind()
                .AddFindCms()
                .AddAdminUserRegistration()
                .AddEmbeddedLocalization<Startup>();

            services.AddHttpContextAccessor();
            services.AddScoped<IXmlSitemapService, XmlSitemapService>();
            services.AddSingleton<ErrorMessageService>();
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

                if (!context.HttpContext.Request.Path.StartsWithSegments("/error"))
                {
                    response.Redirect($"/error?statusCode={statusCode}");
                    await Task.Yield();
                }
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapContent();

                endpoints.MapBlazorHub();

               endpoints.MapControllers();
            });
        }
    }
}
