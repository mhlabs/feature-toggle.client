using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mhlabs.feature_toggle.client
{
    public interface IFeatureToggleClient
    {
        Task<T> Get<T>(string flagName, string userKey, T defaultValue = default(T), CancellationToken cancellationToken = default(CancellationToken));
        Task<T> Get<T>(string flagName, string userKey, IDictionary<string, string> userMetadata = default(IDictionary<string, string>), T defaultValue = default(T), CancellationToken cancellationToken = default(CancellationToken));

        Task<T> GetAll<T>(string userKey, CancellationToken cancellationToken = default(CancellationToken));
        Task<T> GetAll<T>(string userKey, IDictionary<string, string> userMetadata = default(IDictionary<string, string>), CancellationToken cancellationToken = default(CancellationToken));
    }
}
