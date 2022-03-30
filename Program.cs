using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReelJunkies.Services;
using System.Threading.Tasks;

namespace ReelJunkies
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();
            //make a call to the data service, this will run
            //everytime the application runs
            var dataService = host.Services
                                .CreateScope()
                                .ServiceProvider
                                .GetRequiredService<SeedService>();
            //this will perform the seed tasks
            await dataService.ManagedDataAsync();

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
