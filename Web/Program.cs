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
                        .AddJsonFile("development_config.json")
                        .Build());
            })
            .Build()
            .Run();
    }
}