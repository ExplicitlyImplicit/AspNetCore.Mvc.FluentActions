using System;
using System.Collections;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionCollection : IEnumerable<FluentActionBase>
    {
        public List<FluentActionBase> FluentActions { get; internal set; }

        public FluentActionCollection()
        {
            FluentActions = new List<FluentActionBase>();
        }

        public FluentAction Route(string url, HttpMethod httpMethod, string title = null)
        {
            var fluentAction = new FluentAction(httpMethod, url, title);
            FluentActions.Add(fluentAction);
            return fluentAction;
        }

        public FluentAction RouteDelete(string url, string title = null)
        {
            return Route(url, HttpMethod.Delete, title);
        }

        public FluentAction RouteGet(string url, string title = null)
        {
            return Route(url, HttpMethod.Get, title);
        }

        public FluentAction RouteHead(string url, string title = null)
        {
            return Route(url, HttpMethod.Head, title);
        }

        public FluentAction RouteOptions(string url, string title = null)
        {
            return Route(url, HttpMethod.Options, title);
        }

        public FluentAction RoutePatch(string url, string title = null)
        {
            return Route(url, HttpMethod.Patch, title);
        }

        public FluentAction RoutePost(string url, string title = null)
        {
            return Route(url, HttpMethod.Post, title);
        }

        public FluentAction RoutePut(string url, string title = null)
        {
            return Route(url, HttpMethod.Put, title);
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
    }
}
