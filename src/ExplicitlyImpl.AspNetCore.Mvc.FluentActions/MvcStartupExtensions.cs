// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMvcWithFluentActions(
            this IApplicationBuilder app,
            Action<FluentActionCollection> configureFluentActions,
            Action<IRouteBuilder> configureRoutes = null)
        {
            var fluentActions = FluentActionCollection.DefineActions(configureFluentActions);

            return app.UseMvcWithFluentActions(fluentActions, configureRoutes);
        }

        public static IApplicationBuilder UseMvcWithFluentActions(
            this IApplicationBuilder app,
            FluentActionCollection fluentActions,
            Action<IRouteBuilder> configureRoutes = null)
        {
            var controllerDefinitionBuilder = new FluentActionControllerDefinitionBuilder();

            var controllerDefinitions = fluentActions
                .Select(fluentAction => controllerDefinitionBuilder.Build(fluentAction))
                .ToList();

            var context = (FluentActionControllerFeatureProviderContext)app
                .ApplicationServices
                .GetService(typeof(FluentActionControllerFeatureProviderContext));

            context.ControllerDefinitions = controllerDefinitions;

            return app.UseMvc(routes =>
            {
                foreach (var controllerDefinition in controllerDefinitions
                    .Where(controllerDefinition => controllerDefinition.FluentAction.Definition.IsMapRoute))
                {
                    routes.MapRoute(
                        controllerDefinition.Id,
                        controllerDefinition.RouteTemplate.WithoutLeading("/"),
                        new
                        {
                            controller = controllerDefinition.Name.WithoutTrailing("Controller"),
                            action = controllerDefinition.ActionName
                        });
                }

                configureRoutes?.Invoke(routes);
            });
        }
    }

    public static class ApplicationServiceCollectionExtensions
    {
        public static IMvcBuilder AddMvcWithFluentActions(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var context = new FluentActionControllerFeatureProviderContext();
            services.TryAddSingleton(context);

            var fluentActionControllerFeatureProvider = new FluentActionControllerFeatureProvider(context);

            return services
                .AddMvc()
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(fluentActionControllerFeatureProvider);
                });
        }

        public static IMvcBuilder AddMvcWithFluentActions(
            this IServiceCollection services, 
            Action<MvcOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddMvc(setupAction);
        }
    }
}
