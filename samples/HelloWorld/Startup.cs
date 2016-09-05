using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace HelloWorld
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcWithFluentActions();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithFluentActions(actions =>
            {
                actions.RouteGet("/").To(() => "Hello World!");
            });
        }
    }
}
