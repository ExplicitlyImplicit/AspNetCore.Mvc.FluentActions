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
