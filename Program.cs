using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WomanSite;
static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args);

    var defaultBuilder = hostBuilder.ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });

    return defaultBuilder;
}
var hostBuilder = CreateHostBuilder(args);
var host = hostBuilder.Build();
host.Run();