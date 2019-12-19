namespace mhlabs.feature_toggle.client.Services.Responses
{
    public interface IFeatureToggleResponse
    {
        bool Active { get; set; }
        bool Successful { get; set; }
        long TimeStamp { get; }
    }
}
