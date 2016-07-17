using System.Collections;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointCollection : IEnumerable<Endpoint>
    {
        public List<Endpoint> Endpoints { get; internal set; }

        public EndpointCollection()
        {
            Endpoints = new List<Endpoint>();
        }

        public Endpoint Add(HttpMethod httpMethod, string url, string description = null)
        {
            var endpoint = new Endpoint(httpMethod, url, description);
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public Endpoint Add(string url, HttpMethod httpMethod, string description = null)
        {
            var endpoint = new Endpoint(httpMethod, url, description);
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public void Add(Endpoint endpoint)
        {
            Endpoints.Add(endpoint);
        }

        public void Add(IEnumerable<Endpoint> endpoints)
        {
            Endpoints.AddRange(endpoints);
        }

        public IEnumerator<Endpoint> GetEnumerator()
        {
            return Endpoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
