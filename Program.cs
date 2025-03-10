using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BreakoutExtreme
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddScoped(sp => new HttpClient()
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            builder.Services.AddScoped<BrowserService>();
            builder.Services.AddScoped<QueuedHostedService>();
            var host = builder.Build();
            var browserService = host.Services.GetService<BrowserService>();
            await browserService.ConfigureBrowserServer();
            var queuedHostService = host.Services.GetService<QueuedHostedService>();
            var cancellationTokenSource = new CancellationTokenSource();
            await queuedHostService.StartAsync(cancellationTokenSource.Token);
            await host.RunAsync();
        }
    }
}
