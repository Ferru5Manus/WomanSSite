using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
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
            services.AddAuthentication();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new PathString("/Auth"));
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
                    
                });
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
                    // с заданным логином и паролем мы пойдем в базу
                    // если в базе есть пользователь, то всё ок, если нет, то ничего не делаем
                    var usersService = app.ApplicationServices.GetService<AuthController>();
                    if (usersService.Login(credentials))
                    {
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, credentials.name)
                        };
                        // создаем объект ClaimsIdentity
                        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                        // добавляем куки нашему пользователю
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                        try
                        {

                            // перенаправляем на нужную сраницу
                            context.Response.Redirect("/chatPage");
                        }
                        catch(Exception ex){
                            Console.WriteLine(ex);
                        }
                    }

                    await context.Response.WriteAsync(credentials.name);
                });
              
             });
        }
    }
    
}
