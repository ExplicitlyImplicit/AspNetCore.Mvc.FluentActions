using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace MvcTemplate
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddFluentActions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
            app.UseFluentActions(actions =>
            {
                actions
                    .RouteGet("/")
                    .ToView("~/Views/Home/Index.cshtml");

                actions
                    .RouteGet("/Home/About")
                    .UsingViewData()
                    .Do(viewData => viewData["Message"] = "Your application description page.")
                    .ToView("~/Views/Home/About.cshtml");

                actions
                    .RouteGet("/Home/Contact")
                    .UsingViewData()
                    .Do(viewData => viewData["Message"] = "Your contact page.")
                    .ToView("~/Views/Home/Contact.cshtml");

                actions
                    .RouteGet("/Home/Error")
                    .ToView("~/Views/Shared/Error.cshtml");
            });
        }
    }
}
