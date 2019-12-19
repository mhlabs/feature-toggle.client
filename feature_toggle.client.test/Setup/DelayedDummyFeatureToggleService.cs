using mhlabs.feature_toggle.client.Services;
using System.Threading.Tasks;
using System.Threading;
using mhlabs.feature_toggle.client.Services.Responses;

namespace mhlabs.feature_toggle.client.tests
{

    public class DelayedDummyFeatureToggleService : IFeatureToggleService
    {
        public async Task<IFeatureToggleResponse> Get(string flagName, string userKey, bool defaultValue = false, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1000, cancellationToken);
            return new FeatureToggleResponse();
        }
    }
}
