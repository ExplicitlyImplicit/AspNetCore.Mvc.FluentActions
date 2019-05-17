﻿using System;
using System.Diagnostics;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Models;

namespace MvcTemplate
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
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                    .ToView("~/Views/Home/Index.cshtml");

                actions
                    .RouteGet("/Home/Privacy")
                    .ToView("~/Views/Home/Privacy.cshtml");

                actions
                    .RouteGet("/Home/Error")
                    .WithCustomAttribute<ResponseCacheAttribute>(
                        new Type[0],
                        new object[0],
                        new string[] { "Duration", "Location", "NoStore" },
                        new object[] { 0, ResponseCacheLocation.None, true }
                    )
                    .UsingHttpContext()
                    .To(httpContext => new ErrorViewModel { RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier })
                    .ToView("~/Views/Shared/Error.cshtml");
            });

            app.UseMvc();
        }
    }
}
