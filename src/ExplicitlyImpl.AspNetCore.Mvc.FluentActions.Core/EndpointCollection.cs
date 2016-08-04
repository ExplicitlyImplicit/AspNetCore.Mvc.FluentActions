using System.Collections;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class EndpointCollection : IEnumerable<FluentActionBase>
    {
        public List<FluentActionBase> Endpoints { get; internal set; }

        public EndpointCollection()
        {
            Endpoints = new List<FluentActionBase>();
        }

        public EndpointCollection(IEnumerable<IEnumerable<FluentActionBase>> endpointCollections) : this()
        {
            foreach(var endpointCollection in endpointCollections)
            {
                Endpoints.AddRange(endpointCollection);
            }
        }

        public FluentAction Add(HttpMethod httpMethod, string url, string title = null)
        {
            var endpoint = new FluentAction(httpMethod, url, title);
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public FluentAction Add(string url, HttpMethod httpMethod, string title = null)
        {
            var endpoint = new FluentAction(httpMethod, url, title);
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public void Add(FluentActionBase endpoint)
        {
            Endpoints.Add(endpoint);
        }

        public void Add(IEnumerable<FluentActionBase> endpoints)
        {
            Endpoints.AddRange(endpoints);
        }

        public IEnumerator<FluentActionBase> GetEnumerator()
        {
            return Endpoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
