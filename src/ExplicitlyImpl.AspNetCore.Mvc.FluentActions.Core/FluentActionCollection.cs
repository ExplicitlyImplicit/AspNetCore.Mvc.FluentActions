// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionCollection : IEnumerable<FluentActionBase>
    {
        public List<FluentActionBase> FluentActions { get; internal set; }

        public string GroupName { get; internal set; }

        internal FluentActionCollection()
        {
            FluentActions = new List<FluentActionBase>();
        }

        public FluentAction Route(string routeTemplate, HttpMethod httpMethod, string id = null)
        {
            var fluentAction = new FluentAction(httpMethod, routeTemplate, id);
            ConfigureAndAddAction(fluentAction);
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
            ConfigureAndAddAction(fluentAction);
        }

        public void Add(FluentActionCollection fluentActions)
        {
            foreach (var fluentAction in fluentActions)
            {
                ConfigureAndAddAction(fluentAction);
            }
        }

        public void GroupBy(string groupName)
        {
            GroupName = groupName;
        }

        public IEnumerator<FluentActionBase> GetEnumerator()
        {
            return FluentActions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ConfigureAndAddAction(FluentActionBase fluentAction)
        {
            if (GroupName != null)
            {
                fluentAction.Definition.GroupName = GroupName;
            }

            FluentActions.Add(fluentAction);
        }

        public static FluentActionCollection DefineActions(Action<FluentActionCollection> addFluentActions)
        {
            var actionCollection = new FluentActionCollection();

            addFluentActions(actionCollection);

            return actionCollection;
        }
    }
}
