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

        public FluentAction Route(string routeTemplate, HttpMethod httpMethod, string title = null)
        {
            var fluentAction = new FluentAction(httpMethod, routeTemplate, title);
            FluentActions.Add(fluentAction);
            return fluentAction;
        }

        public FluentAction RouteDelete(string routeTemplate, string title = null)
        {
            return Route(routeTemplate, HttpMethod.Delete, title);
        }

        public FluentAction RouteGet(string routeTemplate, string title = null)
        {
            return Route(routeTemplate, HttpMethod.Get, title);
        }

        public FluentAction RouteHead(string routeTemplate, string title = null)
        {
            return Route(routeTemplate, HttpMethod.Head, title);
        }

        public FluentAction RouteOptions(string routeTemplate, string title = null)
        {
            return Route(routeTemplate, HttpMethod.Options, title);
        }

        public FluentAction RoutePatch(string routeTemplate, string title = null)
        {
            return Route(routeTemplate, HttpMethod.Patch, title);
        }

        public FluentAction RoutePost(string routeTemplate, string title = null)
        {
            return Route(routeTemplate, HttpMethod.Post, title);
        }

        public FluentAction RoutePut(string routeTemplate, string title = null)
        {
            return Route(routeTemplate, HttpMethod.Put, title);
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
