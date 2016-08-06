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

namespace SimpleWebApi
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
            services.AddMvcWithFluentActions();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<INoteService, NoteService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseMvcWithFluentActions(FluentActions.AllExternal);

            app.UseMvcWithFluentActions(actions =>
            {
                actions
                    .AddRoute("/api/users", HttpMethod.Get, "List users.")
                    .UsingService<IUserService>()
                    .To(userService => userService.List())
                    .ToView("users/list.cshtml");

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

                //actions
                //    .AddRoute("/api/users/{userId}", HttpMethod.Put)
                //    .UsingService<IUserService>()
                //    .UsingDataModel<UserItem>(model =>  
                //        model.InitiallyBoundFromBody();  
                //        model.BindUrlParameter<int>("userId", user => user.Id); 
                //    )  
                //    .To((userService, user) => userService.Update(user));

                //actions
                //    .AddRoute("/api/users/{userId}", HttpMethod.Get)
                //    .UsingController<UserController>()
                //    .UsingParameter<int>("userId")
                //    .To((userController, userId) => userController.Edit(userId));

                //actions
                //    .AddRoute("/api/users/{userId}", HttpMethod.Get)
                //    .ToController<UserController>("Get");

                //actions
                //    .AddRoute("/api/users/{userId}", HttpMethod.Get)
                //    .UsingService<IUserService>()
                //    .UsingParameter<int>("userId")
                //    .To((userService, userId) => userService.Get(userId))
                //    .UsingService<IJsonUtilsService>()
                //    .To((result, jsonUtilsService) => jsonUtilsService.Encode(result));

                //actions
                //    .AddRoute("/api/users/{userId}", HttpMethod.Get)
                //    .UsingService<IUserService>()
                //    .UsingParameter<int>("userId")
                //    .To((userService, userId) => userService.Get(userId))
                //    .ToJson()
            });
        }
    }
}
