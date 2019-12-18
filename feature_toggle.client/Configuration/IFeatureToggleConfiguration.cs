using System;
using System.Threading;
using System.Threading.Tasks;
using mhlabs.feature_toggle.client.Services.Responses;

namespace mhlabs.feature_toggle.client.Services
{
    public interface IFeatureToggleConfiguration
    {
        double CacheDurationInSeconds { get; }
        string ApiPathFormat { get; }
    }

    public class FeatureToggleConfiguration : IFeatureToggleConfiguration
    {
        public double CacheDurationInSeconds => throw new System.NotImplementedException(); //long.TryParse(Environment.GetEnvironmentVariable("CacheDurationInSeconds"))
        public string ApiPathFormat => throw new System.NotImplementedException();
    }
}
