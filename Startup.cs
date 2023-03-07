using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Net;
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/loginPage");
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();  
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
                endpoints.MapPost("/login", async context => 
                {
                    var user = await context.Request.ReadFromJsonAsync<User>();
                    var am = app.ApplicationServices.GetService<AuthController>();
                    var name = user.name;
                    var key = user.key;
                    if (am.Login(user)==true)
                    {

                        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.name) };
                        // создаем объект ClaimsIdentity
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                        // установка аутентификационных куки
                        
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        await context.Response.WriteAsJsonAsync(true);
                    }

                });
                endpoints.MapPost("/check", async context => {
                    await context.Response.WriteAsJsonAsync(context.User.Identity.Name);
                });
            });
        }
    }
    
}
