using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
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
            services.AddSingleton<DialogueController>();
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
                    user.name = user.name.Replace(" ","");

                    var key = user.key;
                    if (am.Login(user)==true)
                    {
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, user.name)
                        };
                        // ñîçäàåì îáúåêò ClaimsIdentity
                        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                        // äîáàâëÿåì êóêè íàøåìó ïîëüçîâàòåëþ
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                        await context.Response.WriteAsJsonAsync(false);
                    }
                    else
                    {
                        await context.Response.WriteAsJsonAsync(false);
                    }
                });
                endpoints.MapGet("/getName", async context => 
                {
                    await context.Response.WriteAsJsonAsync(context.User.Identity.Name);
                });
                endpoints.MapPost("/getMessage", async context => 
                {
                    var question = await context.Request.ReadFromJsonAsync<Question>();
                    Console.WriteLine(question.id +" "+ question.personName);
                    var dc = app.ApplicationServices.GetService<DialogueController>();
                    await context.Response.WriteAsJsonAsync(dc.GetMessage(question.id,question.personName));
                });
                endpoints.MapPost("/broadcastMessage", async context =>
                {
                    var answer = await context.Request.ReadFromJsonAsync<Answer>();
                    var dc = app.ApplicationServices.GetService<DialogueController>();
                    var ret = dc.GetMessage(answer.questionId+1, context.User.Identity.Name);
                    if (ret != "end")
                    {
                        Console.WriteLine("ans adding");
                        var ans1 = new Answer() { questionId = answer.questionId, userLogin = context.User.Identity.Name, answer = answer.answer };
                        var x = dc.addAnswer(ans1);
                        Console.WriteLine(x);
                    }
                    await context.Response.WriteAsJsonAsync(ret);
                });
            });
        }
    }
    
}
