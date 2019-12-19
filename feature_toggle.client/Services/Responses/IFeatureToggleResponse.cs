namespace mhlabs.feature_toggle.client.Services.Responses
{
    public interface IFeatureToggleResponse
    {
        bool Enabled { get; set; }
        string Error { get; set; }
        long TimeStamp { get; }
    }
}
