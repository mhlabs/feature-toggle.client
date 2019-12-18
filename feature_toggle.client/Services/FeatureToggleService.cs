using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MhLabs.AwsSignedHttpClient;
using mhlabs.feature_toggle.client.Services.Responses;

namespace mhlabs.feature_toggle.client.Services
{
    public class FeatureToggleService : IFeatureToggleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FeatureToggleService> _logger;
        private readonly string pathFormat = "/{0}/{1}/false";

        public FeatureToggleService(HttpClient httpClient, ILogger<FeatureToggleService> logger)
        {
            this._httpClient = httpClient;
            this._logger = logger;

            pathFormat = Environment.GetEnvironmentVariable("FT_PATH_FORMAT") ?? pathFormat;
        }

        public async Task<IFeatureToggleResponse> Get(string flagName, string userKey, bool defaultValue = default(bool), CancellationToken cancellationToken = default) 
        {
            try 
            {
                var url = string.Format(pathFormat, flagName, userKey);
                var response = await _httpClient.SendAsync<FeatureToggleServiceContract>(HttpMethod.Get, url, cancellationToken: cancellationToken);
                
                return new FeatureToggleResponse() 
                {
                    Active = response.Active,
                    Successful = true
                };

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

        internal class FeatureToggleServiceContract
        {
            public bool Active { get; set; }
        }
    }
}
