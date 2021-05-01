using System;
using System.Collections.Generic;

namespace TrueLayerStatusPage.Models
{
    public class LatencyPercentiles
    {
        public double _99 { get; set; }
        public double _90 { get; set; }
        public double _75 { get; set; }
        public double _95 { get; set; }
        public double _50 { get; set; }
    }

    public class Endpoint
    {
        public string endpoint { get; set; }
        public LatencyPercentiles latency_percentiles { get; set; }
        public double availability { get; set; }
        public double provider_error { get; set; }
        public double truelayer_error { get; set; }

        public override string ToString()
        {
            return $"{nameof(endpoint)}: {endpoint}";
        }
    }

    public class Provider
    {
        public string provider_id { get; set; }
        public List<Endpoint> endpoints { get; set; }
        public double availability { get; set; }
        public double provider_error { get; set; }
        public double truelayer_error { get; set; }

        public override string ToString()
        {
            return $"{nameof(provider_id)}: {provider_id}";
        }
    }

    public class Result
    {
        public DateTime timestamp { get; set; }
        public List<Provider> providers { get; set; }
        public double availability { get; set; }
        public double provider_error { get; set; }
        public double truelayer_error { get; set; }
    }

    public class StatusResponse
    {
        public string status { get; set; }
        public List<Result> results { get; set; }
    }
}
