namespace BookBuddy
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IConfiguration Configuration { get; } =
            new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
            .AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true)
            .AddEnvironmentVariables()
            .Build();
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureCmsDefaults()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
