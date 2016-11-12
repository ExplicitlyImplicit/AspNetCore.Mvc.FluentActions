using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleMvcWithViews.Data;
using SimpleMvcWithViews.Models;
using SimpleMvcWithViews.Services;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using SimpleMvcWithViews.Controllers;
using System.Linq;

namespace SimpleMvcWithViews
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvcWithFluentActions();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseSession();

            app.UseMvcWithFluentActions(actions =>
            {
                actions
                    .Route("/helloWorld", HttpMethod.Get)
                    .To(() => "Hello World!");

                actions
                    .Route("/toHome", HttpMethod.Get)
                    .ToMvcController<HomeController>()
                    .ToMvcAction(homeController => homeController.Index());

                actions
                    .Route("/toAbout", HttpMethod.Get)
                    .UsingViewBag()
                    .UsingViewData()
                    .UsingTempData()
                    .Do((viewBag, viewData, tempData) => 
                    {
                        viewData["ViewDataMessage"] = "ViewData message from fluent action.";
                        tempData["TempDataMessage"] = "TempData message from fluent action.";
                        viewBag.ViewBagMessage = "ViewBag message from fluent action.";
                    })
                    .ToView("~/Views/Home/About.cshtml");

                actions
                    .Route("/toError", HttpMethod.Get)
                    .ToView("~/Views/Shared/Error.cshtml");

                actions
                    .Route("/users", HttpMethod.Get, "List users.")
                    .UsingService<IUserService>()
                    .To(userService => userService.List())
                    .ToView("~/views/users/list.cshtml");

                actions
                    .Route("/api/users", HttpMethod.Get, "List users.")
                    .UsingService<IUserService>()
                    .To(userService => userService.List());

                actions
                    .RouteGet("/uploadFile", "Form to upload file.")
                    .ToView("~/Views/Form/UploadFile.cshtml");

                actions
                    .RoutePost("/uploadFile", "Upload file.")
                    .UsingFormFile("file")
                    .To(file => $"Got {file.FileName}!");

                actions
                    .RouteGet("/uploadFiles", "Form to upload files.")
                    .ToView("~/Views/Form/UploadFiles.cshtml");

                actions
                    .RoutePost("/uploadFiles", "Upload files.")
                    .UsingFormFiles("files")
                    .To(files => $"Got {files.Count()} n.o. files!");

                actions
                    .RouteGet("/submitWithAntiForgeryToken", "Form to submit with anti forgery token.")
                    .ToView("~/Views/Form/SubmitWithAntiForgeryToken.cshtml");

                actions
                    .RoutePost("/submitWithAntiForgeryToken", "Submit with anti forgery token.")
                    .ValidateAntiForgeryToken()
                    .To(() => "Got submission!");
            }, routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
