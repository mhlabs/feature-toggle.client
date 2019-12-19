using System;
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

        public Task<IFeatureToggleResponse> Get(string flagName, string userKey, bool defaultValue = default(bool))
        {
            var cacheKey = ToCacheKey(flagName, userKey, defaultValue);
            _logger.LogInformation("Retrieving flag: {Flag} for user: {User} with default value: {DefaultValue}. Cache key: {CacheKey}", flagName, userKey, defaultValue, cacheKey);

            return _cache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_configuration.CacheDurationInSeconds));
                return await GetEntry(flagName, userKey, defaultValue);
            });
        }

        private async Task<IFeatureToggleResponse> GetEntry(string flagName, string userKey, bool defaultValue)
        {
            try
            {
                var cts = new CancellationTokenSource(_configuration.ApiRequestTimeoutMilliseconds);
                return await _service.Get(flagName, userKey, defaultValue, cts.Token);
            }
            catch (Exception ex)
            {
                return HandleException(ex, flagName, userKey, defaultValue);
            }
            
        }

        private IFeatureToggleResponse HandleException(Exception ex, string flagName, string userKey, bool defaultValue)
        {
            if (ex is UnauthorizedAccessException)
            {
                _logger.LogError(ex, "Request UnauthorizedAccessException - Flag: {Flag}. User: {UserKey}");    
            }
            else if (ex is TimeoutException)
            {
                _logger.LogError(ex, "Request TimeoutException - Flag: {Flag}. User: {UserKey}");    
            }
            else
            {
                _logger.LogError(ex, "Request Exception - Flag: {Flag}. User: {UserKey}");
            }

            return new FeatureToggleResponse() 
            {
                Active = defaultValue,
                Successful = false
            };
        }

        private static string ToCacheKey(string flagName, string userKey, bool defaultValue)
        {
            return $"{flagName}_{userKey}_{defaultValue}";
        }
    }
}
