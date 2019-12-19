using System;
using Xunit;
using mhlabs.feature_toggle.client.Client;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using mhlabs.feature_toggle.client.Services;
using Microsoft.Extensions.Caching.Memory;
using AutoFixture;
using System.Threading.Tasks;
using Shouldly;
using System.Threading;
using mhlabs.feature_toggle.client.Services.Responses;

namespace mhlabs.feature_toggle.client.tests
{
    public class FeatureToggleClientTests
    {
        private readonly Mock<IFeatureToggleService> _service = new Mock<IFeatureToggleService>();
        private readonly Fixture _fixture = new Fixture();
        private readonly TestConfig _config = new TestConfig();
        
        public IFeatureToggleClient CreateClient()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            return new FeatureToggleClient(_service.Object, _config.Configuration, cache, NullLogger<FeatureToggleClient>.Instance);
        }

        [Fact]
        public async Task Should_Return_Response_When_SuccesfulAsync()
        {
            // Arrange
            var client = CreateClient();

            _config.Response.Active = true;
            _config.Response.Successful = true;
            
            _service.Setup(x => x.Get(_config.FlagName, _config.UserKey, _config.DefaultValue, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(_config.Response);

            // Act
            var response = await client.Get(_config.FlagName, _config.UserKey, _config.DefaultValue);
            
            // Assert
            response.Active.ShouldBe(true);
            response.Successful.ShouldBe(true);
        }

        [Fact]
        public void Should_Return_Response_When_Exception()
        {

        }

        [Fact]
        public void Should_Return_Response_When_Timeout()
        {

        }

        [Fact]
        public void Should_Return_Response_When_Cached()
        {

        }
    }

    internal class TestConfig 
    {
        private static readonly Fixture _fixture = new Fixture();

        public IFeatureToggleConfiguration Configuration = _fixture.Create<FeatureToggleConfiguration>();
        public IFeatureToggleResponse Response = new FeatureToggleResponse();
        public string FlagName = _fixture.Create<string>();
        public string UserKey = _fixture.Create<string>();
        public bool DefaultValue = _fixture.Create<bool>();

        public TestConfig()
        {
            
        }
    }
}
