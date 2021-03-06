﻿using System;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Annotations;

namespace MvcWithSwagger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddMvc()
                .AddFluentActions()
                .AddFluentActions() // Test multiple calls to AddFluentActions
                ;

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<INoteService, NoteService>();

            services.AddSwaggerDocument(config =>
            {
                config.Title = "API docs";
                config.Version = "0.1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseFluentActions(actions =>
            {
                actions
                    .RouteGet("/")
                    .WithCustomAttribute<OpenApiTagsAttribute>(
                        new Type[] { typeof(string[]) },
                        new object[] { new string[] { "Hello World" } }
                    )
                    .To(() => "Hello World!");

                actions.Add(UserActions.All);
                actions.Add(NoteActions.All);
            });

            // Test multiple calls to UseFluentActions
            app.UseFluentActions(actions =>
            {
                actions
                    .RouteGet("/starlord")
                    .WithCustomAttribute<OpenApiTagsAttribute>(
                        new Type[] { typeof(string[]) },
                        new object[] { new string[] { "Hello Starlord" } }
                    )
                    .To(() => "Hello Starlord!");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseOpenApi(config =>
            {
                config.Path = "/api/docs/api.json";
            });

            app.UseSwaggerUi3(config =>
            {
                config.DocumentPath = "/api/docs/api.json";
                config.Path = "/api/docs";
            });
        }
    }
}
