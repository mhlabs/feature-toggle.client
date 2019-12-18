using System.Threading;
using System.Threading.Tasks;
using mhlabs.feature_toggle.client.Services.Responses;

namespace mhlabs.feature_toggle.client.Services
{
    public interface IFeatureToggleService
    {
        Task<IFeatureToggleResponse> Get(string flagName, string userKey, bool defaultValue = default(bool), CancellationToken cancellationToken = default(CancellationToken));
    }
}
