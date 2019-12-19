# feature-toggle.client
mhlabs.feature-toggle.client

## Setup

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

## Usage

### Retrieve flag for user 
```
public class MyService
{   
    private readonly IFeatureToggleClient _client;
    private readonly IMyOldService _oldService;
    private readonly IMyNewService _newService;

    public MyService (IFeatureToggleClient client, IMyOldService oldService, IMyNewService newService)
    {
        _client = client;
        _oldService = oldService;
        _newService = newService;
    }

    public async Task<ServiceResponse> GetForMember(string userKey)
    {
        var flag = await _client.Get("my-service", userKey, false);

        return flag.Enabled ?
            await _newService.Get() :
            await _oldService.Get();
    }
}
```

### Contract
```
public interface IFeatureToggleResponse
{
    bool Active { get; set; }
    string Error { get; set; }
    long TimeStamp { get; }
}
```

