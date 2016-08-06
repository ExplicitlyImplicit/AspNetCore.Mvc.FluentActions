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

        public FluentActionCollection(IEnumerable<IEnumerable<FluentActionBase>> fluentActionCollections) : this()
        {
            foreach(var fluentActionCollection in fluentActionCollections)
            {
                FluentActions.AddRange(fluentActionCollection);
            }
        }

        public FluentAction Add(HttpMethod httpMethod, string url, string title = null)
        {
            var fluentAction = new FluentAction(httpMethod, url, title);
            FluentActions.Add(fluentAction);
            return fluentAction;
        }

        public FluentAction AddRoute(string url, HttpMethod httpMethod, string title = null)
        {
            var fluentAction = new FluentAction(httpMethod, url, title);
            FluentActions.Add(fluentAction);
            return fluentAction;
        }

        public void Add(FluentActionBase fluentAction)
        {
            FluentActions.Add(fluentAction);
        }

        public void Add(IEnumerable<FluentActionBase> fluentActions)
        {
            FluentActions.AddRange(fluentActions);
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
