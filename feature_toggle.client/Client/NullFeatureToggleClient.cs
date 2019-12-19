using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using mhlabs.feature_toggle.client.Services.Responses;

namespace mhlabs.feature_toggle.client.Client
{
    public class NullFeatureToggleClient : IFeatureToggleClient
    {
        public async Task<IFeatureToggleResponse> Get(string flagName, string userKey, bool defaultValue = false)
        {
            await Task.CompletedTask;
            
            return new FeatureToggleResponse();
        }

        public static IFeatureToggleClient Instance => new NullFeatureToggleClient();
    }
}
