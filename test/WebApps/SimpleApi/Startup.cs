﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace SimpleApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddFluentActions();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<INoteService, NoteService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseFluentActions(actions =>
            {
                actions.RouteGet("/").To(() => "Hello World!");

                actions.Add(UserActions.All);
                actions.Add(NoteActions.All);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
