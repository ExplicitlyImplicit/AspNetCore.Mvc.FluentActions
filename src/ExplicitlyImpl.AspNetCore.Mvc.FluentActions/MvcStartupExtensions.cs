﻿using Microsoft.AspNetCore.Builder;
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
        public static IApplicationBuilder UseMvcWithFluentEndpoints(
            this IApplicationBuilder app,
            Action<EndpointCollection> configureEndpoints,
            Action<IRouteBuilder> configureRoutes = null)
        {
            var endpoints = new EndpointCollection();

            configureEndpoints(endpoints);

            return app.UseMvcWithFluentEndpoints(endpoints, configureRoutes);
        }

        public static IApplicationBuilder UseMvcWithFluentEndpoints(
            this IApplicationBuilder app,
            EndpointCollection endpoints,
            Action<IRouteBuilder> configureRoutes = null)
        {
            var controllerDefinitionBuilder = new EndpointControllerDefinitionBuilder();

            var controllerDefinitions = endpoints
                .Select(endpoint => controllerDefinitionBuilder.Create(endpoint))
                .ToList();

            var context = (EndpointControllerFeatureProviderContext)app
                .ApplicationServices
                .GetService(typeof(EndpointControllerFeatureProviderContext));

            context.ControllerDefinitions = controllerDefinitions;

            return app.UseMvc(routes =>
            {
                foreach (var controllerDefinition in controllerDefinitions)
                {
                    routes.MapRoute(
                        controllerDefinition.Id,
                        controllerDefinition.Url,
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
        public static IMvcBuilder AddMvcWithFluentEndpoints(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var context = new EndpointControllerFeatureProviderContext();
            services.TryAddSingleton(context);

            var endpointControllerFeatureProvider = new EndpointControllerFeatureProvider(context);

            return services
                .AddMvc()
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(endpointControllerFeatureProvider);
                });
        }

        public static IMvcBuilder AddMvcWithFluentEndpoints(this IServiceCollection services, Action<MvcOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddMvc(setupAction);
        }
    }
}