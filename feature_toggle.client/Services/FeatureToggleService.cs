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
        private readonly IFeatureToggleConfiguration _configuration;

        public FeatureToggleService(HttpClient httpClient, IFeatureToggleConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IFeatureToggleResponse> Get(string flagName, string userKey, bool defaultValue = default(bool), CancellationToken cancellationToken = default) 
        {
            var url = string.Format(_configuration.ApiPathFormat, flagName, userKey);
            var response = await _httpClient.SendAsync<FeatureToggleServiceContract>(HttpMethod.Get, url, cancellationToken: cancellationToken);
            
            return new FeatureToggleResponse() 
            {
                Active = response.Active,
                Successful = true
            };
        }

        internal class FeatureToggleServiceContract
        {
            public bool Active { get; set; }
        }
    }
}
