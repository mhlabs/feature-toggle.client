namespace mhlabs.feature_toggle.client.Services.Responses
{
    public interface IFeatureToggleResponse
    {
        bool Active { get; }
        bool Successful { get; }
        long TimeStamp { get; }
    }
}
