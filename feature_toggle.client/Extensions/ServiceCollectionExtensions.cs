using System;
using mhlabs.feature_toggle.client.Client;
using mhlabs.feature_toggle.client.Services;
using MhLabs.AwsSignedHttpClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace mhlabs.feature_toggle.client.Configuration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers client with RetryLevel.Read and UseCircuitBreaker = true
        /// </summary>
        public static IServiceCollection AddFeatureToggleClient(this IServiceCollection services, string baseUrl)
        {
            return services.AddFeatureToggleClient(new HttpOptions
            {
                BaseUrl = baseUrl,
                RetryLevel = RetryLevel.Read,
                UseCircuitBreaker = true
            });
        }
        public static IServiceCollection AddFeatureToggleClient(this IServiceCollection services, HttpOptions options)
        {
            services.AddSingleton<IFeatureToggleClient, FeatureToggleClient>();
            services.AddSingleton<IFeatureToggleConfiguration, FeatureToggleConfiguration>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSignedHttpClient<IFeatureToggleService, FeatureToggleService>(options);
            return services;
        }
    }
}
