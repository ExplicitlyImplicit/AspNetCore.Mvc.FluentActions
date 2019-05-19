using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using Microsoft.AspNetCore.Mvc;

namespace SimpleApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddFluentActions()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<INoteService, NoteService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFluentActions(actions =>
            {
                actions.RouteGet("/").To(() => "Hello World!");

                actions.Add(UserActions.All);
                actions.Add(NoteActions.All);
            });
            app.UseMvc();
        }
    }
}
