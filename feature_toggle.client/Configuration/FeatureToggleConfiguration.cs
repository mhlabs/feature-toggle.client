using System;
using Microsoft.Extensions.Logging;

namespace mhlabs.feature_toggle.client.Services
{
    public class FeatureToggleConfiguration : IFeatureToggleConfiguration
    {
        private const double DefaultCacheDurationSeconds = 60;
        private const int DefaultApiRequestTimeoutMilliseconds = 500;
        private const string DefaultApiPathFormat = "/{0}/{1}/false";

        public double CacheDurationInSeconds => long.TryParse(Environment.GetEnvironmentVariable("CacheDurationInSeconds"), out var duration) ?
                    duration :
                    DefaultCacheDurationSeconds;

        public int ApiRequestTimeoutMilliseconds => int.TryParse(Environment.GetEnvironmentVariable("DefaultApiRequestTimeoutMilliseconds"), out var duration) ?
                    duration :
                    DefaultApiRequestTimeoutMilliseconds;
        
        public string ApiPathFormat => Environment.GetEnvironmentVariable("ApiPathFormat") ?? DefaultApiPathFormat;
    }
}
