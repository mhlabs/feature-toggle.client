using System;

namespace mhlabs.feature_toggle.client.Services
{
    public interface IFeatureToggleConfiguration
    {
        double CacheDurationInSeconds { get; }
        string ApiPathFormat { get; }
        string ApiBaseUrl { get; }
        int ApiRequestTimeoutMilliseconds { get; }
    }
}
