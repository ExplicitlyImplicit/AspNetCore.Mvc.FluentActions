// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFluentActions(
            this IApplicationBuilder app,
            Action<FluentActionCollection> addFluentActions)
        {
            if (addFluentActions == null)
            {
                throw new ArgumentNullException(nameof(addFluentActions));
            }

            var fluentActions = FluentActionCollection.DefineActions(addFluentActions);

            return app.UseFluentActions(fluentActions);
        }

        public static IApplicationBuilder UseFluentActions(
            this IApplicationBuilder app,
            Action<FluentActionCollectionConfigurator> configureFluentActions,
            Action<FluentActionCollection> addFluentActions)
        {
            if (addFluentActions == null)
            {
                throw new ArgumentNullException(nameof(addFluentActions));
            }

            var fluentActions = FluentActionCollection.DefineActions(configureFluentActions, addFluentActions);

            return app.UseFluentActions(fluentActions);
        }

        public static IApplicationBuilder UseFluentActions(
            this IApplicationBuilder app,
            FluentActionCollection fluentActions)
        {
            if (fluentActions == null)
            {
                throw new ArgumentNullException(nameof(fluentActions));
            }

            var controllerDefinitionBuilder = new FluentActionControllerDefinitionBuilder();

            var controllerDefinitions = fluentActions
                .Select(fluentAction => controllerDefinitionBuilder.Build(fluentAction))
                .ToList();

            if (!controllerDefinitions.Any())
            {
                return app;
            }

            var context = (FluentActionControllerFeatureProviderContext)app
                .ApplicationServices
                .GetService(typeof(FluentActionControllerFeatureProviderContext));

            if (context == null)
            {
                throw new Exception("Could not find a feature provider for fluent actions, did you remember to call app.AddMvc().AddFluentActions()?");
            }

            context.ControllerDefinitions = controllerDefinitions;

            var routes = new RouteBuilder(app)
            {
                DefaultHandler = app.ApplicationServices.GetRequiredService<MvcRouteHandler>(),
            };

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

            return app.UseRouter(routes.Build());
        }
    }

    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddFluentActions(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Stop if our feature provider has already been added (AddFluentActions has already been called)
            if (builder.Services.Any(s => s.ServiceType == typeof(FluentActionControllerFeatureProviderContext)))
            {
                return builder;
            }

            var context = new FluentActionControllerFeatureProviderContext();
            builder.Services.TryAddSingleton(context);

            var fluentActionControllerFeatureProvider = new FluentActionControllerFeatureProvider(context);

            return builder.ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(fluentActionControllerFeatureProvider);
            });
        }
    }
}
