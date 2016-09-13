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
                    .ToController<HomeController>()
                    .ToAction(homeController => homeController.Index());

                actions
                    .Route("/toAbout", HttpMethod.Get)
                    .UsingViewData()
                    .UsingTempData()
                    .Do((viewData, tempData) => 
                    {
                        viewData["Message"] = "Custom ViewData message.";
                        tempData["Text"] = "Custom TempData message.";
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
            }, routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
