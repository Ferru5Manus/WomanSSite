using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;
using WomanSite.Controllers;
using WomanSite.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                //Adding html
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
                endpoints.MapGet("/chatPage", async context =>
                {
                    string page = File.ReadAllText("Site/chat.html");
                    await context.Response.WriteAsync(page);
                }).RequireAuthorization();
                //Adding css
                endpoints.MapGet("css/start.css",async context=>
                {
                    string page = File.ReadAllText("Site/css/start.css");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapGet("css/login.css", async context =>
                {
                    string page = File.ReadAllText("Site/css/login.css");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapGet("css/normalize.css", async context =>
                {
                    string page = File.ReadAllText("Site/css/normalize.css");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapGet("css/chat.css", async context =>
                {
                    string page = File.ReadAllText("Site/css/chat.css");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapGet("css/default.css", async context =>
                {
                    string page = File.ReadAllText("Site/css/default.css");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapGet("css/message.css", async context =>
                {
                    string page = File.ReadAllText("Site/css/message.css");
                    await context.Response.WriteAsync(page);
                });
                //Adding logic
                endpoints.MapPost("/login", async context => 
                {
                    
                    var credentials = await context.Request.ReadFromJsonAsync<User>();
                    AuthController? lm = app.ApplicationServices.GetService<AuthController>();
                    if (lm.Login(credentials) == true)
                    {
                        Console.WriteLine("response true");
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, credentials.name)
                        };
                        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                        
                        await context.Response.WriteAsJsonAsync(true);
                    }
                    else
                    {
                        Console.WriteLine("response False");
                        await context.Response.WriteAsJsonAsync(false);
                    }
                });

            });
        }
    }
    
}
