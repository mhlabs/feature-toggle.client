using System;
using Newtonsoft.Json;

namespace mhlabs.feature_toggle.client.Services.Responses
{
    public class FeatureToggleResponse : IFeatureToggleResponse
    {
        public bool Enabled { get; set; }
        public string Error { get; set; }
        public long TimeStamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
