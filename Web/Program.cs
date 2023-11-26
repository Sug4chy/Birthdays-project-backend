namespace Web;

internal static class Program
{
    public static void Main()
    {
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>()
                    .UseConfiguration(new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build());
            })
            .Build()
            .Run();
    }
}