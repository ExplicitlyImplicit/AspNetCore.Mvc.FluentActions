using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add framework services.
            services.AddMvcWithFluentActions();

            // Add application services.
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvcWithFluentActions(actions =>
            {
                actions
                    .AddRoute("/helloWorld", HttpMethod.Get)
                    .To(() => "Hello World!");

                actions
                    .AddRoute("/users", HttpMethod.Get, "List users.")
                    .UsingService<IUserService>()
                    .To(userService => userService.List())
                    .ToView("~/views/users/list.cshtml");

                actions
                    .AddRoute("/api/users", HttpMethod.Get, "List users.")
                    .UsingService<IUserService>()
                    .To(userService => userService.List());

                actions
                    .AddRoute("/api/users", HttpMethod.Post)
                    .UsingService<IUserService>()
                    .UsingBody<UserItem>()
                    .To((userService, user) => userService.Add(user));

                actions
                    .AddRoute("/api/users/{userId}", HttpMethod.Get)
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .To((userService, userId) => userService.Get(userId));

                actions
                    .AddRoute("/api/users/{userId}", HttpMethod.Put)
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .UsingBody<UserItem>()
                    .To((userService, userId, user) => userService.Update(userId, user));

                actions
                    .AddRoute("/api/users/{userId}", HttpMethod.Delete)
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .To((userService, userId) => userService.Remove(userId));
            }, routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
