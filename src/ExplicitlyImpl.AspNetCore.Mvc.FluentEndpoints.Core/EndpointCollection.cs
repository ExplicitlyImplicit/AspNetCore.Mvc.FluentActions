using System.Collections;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointCollection : IEnumerable<EndpointBase>
    {
        public List<EndpointBase> Endpoints { get; internal set; }

        public EndpointCollection()
        {
            Endpoints = new List<EndpointBase>();
        }

        public EndpointCollection(IEnumerable<IEnumerable<EndpointBase>> endpointCollections) : this()
        {
            foreach(var endpointCollection in endpointCollections)
            {
                Endpoints.AddRange(endpointCollection);
            }
        }

        public Endpoint Add(HttpMethod httpMethod, string url, string title = null)
        {
            var endpoint = new Endpoint(httpMethod, url, title);
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public Endpoint Add(string url, HttpMethod httpMethod, string title = null)
        {
            var endpoint = new Endpoint(httpMethod, url, title);
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public void Add(EndpointBase endpoint)
        {
            Endpoints.Add(endpoint);
        }

        public void Add(IEnumerable<EndpointBase> endpoints)
        {
            Endpoints.AddRange(endpoints);
        }

        public IEnumerator<EndpointBase> GetEnumerator()
        {
            return Endpoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
