using Blazor.Analytics;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using RandomHoliday.Services;

namespace RandomHoliday
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGoogleAnalytics("G-XPJEFHLTRB");
            services.AddRazorPages();
            services
                .AddServerSideBlazor(options =>
                {
                    options.DetailedErrors = true;
                })
                .AddHubOptions(options =>
                {
                    // Upping this to make sure we can send a 5mb file using SignalR. Only needed to test localstorage.
                    // Size should not be changed in production.
                    options.MaximumReceiveMessageSize = 102400000;
                    options.EnableDetailedErrors = true;
                });

            services.AddMudServices();
            services.AddBlazoredLocalStorage();
            services.AddScoped<GeoApiService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
