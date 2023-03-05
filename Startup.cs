using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;
using WomanSite.Controllers;
namespace WomanSite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddSingleton<AuthController>();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new PathString("/auth"));
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            List<Claim> claims;

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapGet("/", async context =>
                {
                    string page = File.ReadAllText("Site/start.html");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapGet("/loginPage", async context => 
                {

                    string page = File.ReadAllText("Site/login.html");
                    await context.Response.WriteAsync(page);
                });
            });
        }
    }
    
}
