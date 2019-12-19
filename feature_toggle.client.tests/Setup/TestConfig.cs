using Moq;
using mhlabs.feature_toggle.client.Services;
using AutoFixture;
using mhlabs.feature_toggle.client.Services.Responses;

namespace mhlabs.feature_toggle.client.tests
{

    public class TestConfig 
    {
        private static readonly Fixture _fixture = new Fixture();

        public IFeatureToggleConfiguration Configuration;
        public IFeatureToggleResponse Response = new FeatureToggleResponse();
        public string FlagName = _fixture.Create<string>();
        public string UserKey = _fixture.Create<string>();
        public bool DefaultValue = _fixture.Create<bool>();

        public TestConfig(int? apiRequestTimeoutMilliseconds = null)
        {
            var config = new Mock<IFeatureToggleConfiguration>();
            config.Setup(x => x.ApiRequestTimeoutMilliseconds).Returns(apiRequestTimeoutMilliseconds ?? 500);
            config.Setup(x => x.CacheDurationInSeconds).Returns(60);
            Configuration = config.Object;
        }
    }
}
