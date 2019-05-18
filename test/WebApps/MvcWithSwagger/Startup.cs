using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Annotations;
using NSwag.AspNetCore;
using NSwag.SwaggerGeneration.Processors.Security;

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

            // Test multiple calls to AddFluentActions
            services.AddMvc().AddFluentActions().AddFluentActions();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<INoteService, NoteService>();

            services.AddSwaggerDocument(config =>
            {
                config.Title = "API docs";
                config.Version = "0.1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseFluentActions(actions =>
            {
                actions
                    .RouteGet("/")
                    .WithCustomAttribute<SwaggerTagsAttribute>(
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
                    .WithCustomAttribute<SwaggerTagsAttribute>(
                        new Type[] { typeof(string[]) },
                        new object[] { new string[] { "Hello Starlord" } }
                    )
                    .To(() => "Hello Starlord!");
            });

            app.UseMvc();

            app.UseSwagger(config =>
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
