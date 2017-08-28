using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace HelloWorld
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentActions();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseFluentActions(actions =>
            {
                actions.RouteGet("/").To(() => "Hello World!");
            });
        }
    }
}
