// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionCollection : IEnumerable<FluentActionBase>
    {
        public List<FluentActionBase> FluentActions { get; internal set; }

        private FluentActionCollection()
        {
            FluentActions = new List<FluentActionBase>();
        }

        public FluentAction Route(string routeTemplate, HttpMethod httpMethod, string id = null)
        {
            var fluentAction = new FluentAction(httpMethod, routeTemplate, id);
            FluentActions.Add(fluentAction);
            return fluentAction;
        }

        public FluentAction RouteDelete(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Delete, id);
        }

        public FluentAction RouteGet(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Get, id);
        }

        public FluentAction RouteHead(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Head, id);
        }

        public FluentAction RouteOptions(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Options, id);
        }

        public FluentAction RoutePatch(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Patch, id);
        }

        public FluentAction RoutePost(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Post, id);
        }

        public FluentAction RoutePut(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Put, id);
        }

        public void Add(FluentActionBase fluentAction)
        {
            FluentActions.Add(fluentAction);
        }

        public void Add(FluentActionCollection fluentActions)
        {
            foreach (var fluentAction in fluentActions)
            {
                FluentActions.Add(fluentAction);
            }
        }

        public IEnumerator<FluentActionBase> GetEnumerator()
        {
            return FluentActions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static FluentActionCollection DefineActions(Action<FluentActionCollection> addFluentActions)
        {
            var actionCollection = new FluentActionCollection();

            addFluentActions(actionCollection);

            return actionCollection;
        }
    }
}
