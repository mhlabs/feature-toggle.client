using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using mhlabs.feature_toggle.client.Services.Responses;

namespace mhlabs.feature_toggle.client
{
    public interface IFeatureToggleClient
    {
        Task<IFeatureToggleResponse> Get(string flagName, string userKey, bool defaultValue = default(bool));
        // Task<T> Get<T>(string flagName, string userKey, IDictionary<string, string> userMetadata = default(IDictionary<string, string>), T defaultValue = default(T), CancellationToken cancellationToken = default(CancellationToken));

        // Task<T> GetAll<T>(string userKey, CancellationToken cancellationToken = default(CancellationToken));
        // Task<T> GetAll<T>(string userKey, IDictionary<string, string> userMetadata = default(IDictionary<string, string>), CancellationToken cancellationToken = default(CancellationToken));
    }
}
