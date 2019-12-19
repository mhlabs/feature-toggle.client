# feature-toggle.client
mhlabs.feature-toggle.client

## Usage

### Environment variables
- ApiPathFormat: _default: "/{0}/{1}/false"_
- ApiRequestTimeoutMilliseconds: _default: 500_
- CacheDurationInSeconds: _default: 60_

### IServiceCollection

Extension methods look like this:
```
public static class ServiceCollectionExtensions
{
    /// Registers client with RetryLevel.Read and UseCircuitBreaker = true
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
        services.AddSignedHttpClient<IFeatureToggleService, FeatureToggleService>(options);
        return services;
    }
}
```

#### With Base only + retries & circuit breaker
```
serviceCollection.AddFeatureToggleClient("https://api.feature-service.com");
```

#### With HttpOptions
```
var options = new HttpOptions ()
{
    BaseUrl = "https://api.feature-service.com",
    RetryLevel = RetryLevel.Read,
    UseCircuitBreaker = true
}

serviceCollection.AddFeatureToggleClient(options);
```

