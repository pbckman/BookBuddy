using Serilog;

namespace BookBuddy
{
    public class Program
    {
        //public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration).WriteTo.Console().CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }
        public static IConfiguration Configuration { get; } =
            new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
            .AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddConfiguration(Configuration);
                })
                .ConfigureCmsDefaults()
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
