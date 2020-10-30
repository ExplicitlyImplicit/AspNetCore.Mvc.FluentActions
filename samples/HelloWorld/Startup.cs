using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HelloWorld
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(config => config.EnableEndpointRouting = false)
                .AddFluentActions();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseFluentActions(actions =>
            {
                actions.RouteGet("/").To(() => "Hello World!");
            });

            app.UseMvc();
        }
    }
}
