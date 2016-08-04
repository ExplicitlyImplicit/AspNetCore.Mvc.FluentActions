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
using ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints;

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
            services.AddMvcWithFluentEndpoints();

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

            app.UseMvcWithFluentEndpoints(endpoints =>
            {
                endpoints
                    .Add("/helloWorld", HttpMethod.Get)
                    .HandledBy(() => "Hello World!");

                endpoints
                    .Add("/users", HttpMethod.Get, "List users.")
                    .UsingService<IUserService>()
                    .HandledBy(userService => userService.List())
                    .RenderedBy("~/views/users/list.cshtml");

                endpoints
                    .Add("/api/users", HttpMethod.Get, "List users.")
                    .UsingService<IUserService>()
                    .HandledBy(userService => userService.List());

                endpoints
                    .Add("/api/users", HttpMethod.Post)
                    .UsingService<IUserService>()
                    .UsingBody<UserItem>()
                    .HandledBy((userService, user) => userService.Add(user));

                endpoints
                    .Add("/api/users/{userId}", HttpMethod.Get)
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .HandledBy((userService, userId) => userService.Get(userId));

                endpoints
                    .Add("/api/users/{userId}", HttpMethod.Put)
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .UsingBody<UserItem>()
                    .HandledBy((userService, userId, user) => userService.Update(userId, user));

                endpoints
                    .Add("/api/users/{userId}", HttpMethod.Delete)
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .HandledBy((userService, userId) => userService.Remove(userId));
            }, routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
