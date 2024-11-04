using BookBuddy.Business.Services;
using BookBuddy.Business.Services.Interfaces;
using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Localization;
using System.Globalization;


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
                .AddAdminUserRegistration()
                .AddEmbeddedLocalization<Startup>();

            services.AddHttpContextAccessor();
            services.AddScoped<IXmlSitemapService, XmlSitemapService>();
            services.AddSingleton<ErrorMessageService>();
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

                // Hämta den aktuella kulturen från URL-segment eller använd default "en" om inget segment finns
                var culture = context.HttpContext.Request.Path.Value?.Split('/').FirstOrDefault(s => s == "sv" || s == "en") ?? "en";

                // Om ingen kultur hittas i URL:en, använd standardkulturen (engelska)
                if (string.IsNullOrEmpty(culture))
                {
                    culture = "en";
                }

                // Bygg om URL:en med rätt språksegment och felkod
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
                endpoints.MapContent();

                endpoints.MapBlazorHub();

               endpoints.MapControllers();
            });
        }
    }
}
