using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using mhlabs.feature_toggle.client.Services;
using mhlabs.feature_toggle.client.Services.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace mhlabs.feature_toggle.client
{
    public class FeatureToggleClient : IFeatureToggleClient
    {
        private readonly ILogger<FeatureToggleClient> _logger;
        private readonly IFeatureToggleService _service;
        private readonly IMemoryCache _cache;
        private readonly IFeatureToggleConfiguration _configuration;

        public FeatureToggleClient(IFeatureToggleService service, IFeatureToggleConfiguration configuration, IMemoryCache cache, ILogger<FeatureToggleClient> logger)
        {
            _logger = logger;
            _cache = cache;
            _service = service;
            _configuration = configuration;
        }

        public Task<IFeatureToggleResponse> Get(string flagName, string userKey, bool defaultValue = default(bool), CancellationToken cancellationToken = default)
        {
            var cacheKey = ToCacheKey(flagName, userKey, defaultValue);
            _logger.LogInformation("Retrieving flag: {Flag} for user: {User} with default value: {DefaultValue}. Cache key: {CacheKey}", flagName, userKey, defaultValue, cacheKey);

            return _cache.GetOrCreateAsync(cacheKey, async cacheEntry => {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_configuration.CacheDurationInSeconds));
                return await _service.Get(flagName, userKey, defaultValue, cancellationToken);
            });
        }

        private static string ToCacheKey(string flagName, string userKey, bool defaultValue)
        {
            return $"{flagName}_{userKey}_{defaultValue}";
        }
    }
}
