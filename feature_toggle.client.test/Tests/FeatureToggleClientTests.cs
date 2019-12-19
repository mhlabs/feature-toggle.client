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
using Microsoft.Extensions.DependencyInjection;
using mhlabs.feature_toggle.client.Configuration;

namespace mhlabs.feature_toggle.client.Test
{
    public class FeatureToggleClientTests
    {
        private readonly Mock<IFeatureToggleService> _service = new Mock<IFeatureToggleService>();
        private readonly Fixture _fixture = new Fixture();
        
        public IFeatureToggleClient CreateClient(TestConfig config = null, IFeatureToggleService service = null)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var testConfig = config ?? new TestConfig();

            return new FeatureToggleClient(service ?? _service.Object, testConfig.Configuration, cache, NullLogger<FeatureToggleClient>.Instance);
        }

        [Fact]
        public void Can_Resolve_Feature_Toggle_Client()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddFeatureToggleClient("http://www.google.com");

            var provider = serviceCollection.BuildServiceProvider();

            // Act
            var instance = provider.GetService<IFeatureToggleClient>();

            // Assert
            instance.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Return_Response_When_SuccesfulAsync()
        {
            // Arrange
            var config = new TestConfig();
            config.Response.Enabled = true;
            config.Response.Error = null;
            
            _service.Setup(x => x.Get(config.FlagName, config.UserKey, config.DefaultValue, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(config.Response);

            // Act
            var client = CreateClient(config);
            var response = await client.Get(config.FlagName, config.UserKey, config.DefaultValue);
            
            // Assert
            response.Enabled.ShouldBeTrue();
            response.Error.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Return_Response_When_Exception()
        {
            // Arrange
            var config = new TestConfig();
            config.Response.Enabled = true;
            config.Response.Error = null;


            _service.Setup(x => x.Get(config.FlagName, config.UserKey, config.DefaultValue, It.IsAny<CancellationToken>()))
                                .ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var client = CreateClient(config);
            var response = await client.Get(config.FlagName, config.UserKey, config.DefaultValue);
            
            // Assert
            response.Enabled.ShouldBe(config.DefaultValue);
            response.Error.ShouldBe(typeof(UnauthorizedAccessException).Name);
        }

        [Fact]
        public async Task Should_Return_Response_When_TaskCanceledException()
        {
            // Arrange
            var timeout = 1;
            var config = new TestConfig(apiRequestTimeoutMilliseconds: timeout);

            // Act
            var client = CreateClient(config, new DelayedDummyFeatureToggleService());
            var response = await client.Get(config.FlagName, config.UserKey, config.DefaultValue);
            
            // Assert
            response.Error.ShouldBe(typeof(TaskCanceledException).Name);
            response.Enabled.ShouldBe(config.DefaultValue);
        }

        [Fact]
        public async Task Should_Return_Response_When_CachedAsync()
        {
            // Arrange
            var config = new TestConfig();
            config.Response.Enabled = true;
            config.Response.Error = null;
            
            _service.Setup(x => x.Get(config.FlagName, config.UserKey, config.DefaultValue, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(config.Response);

            // Act
            var client = CreateClient(config);
            var response1 = await client.Get(config.FlagName, config.UserKey, config.DefaultValue);
            var response2 = await client.Get(config.FlagName, config.UserKey, config.DefaultValue);
            
            // Assert
            response1.Enabled.ShouldBeTrue();
            response1.Error.ShouldBeNull();
            
            response2.Enabled.ShouldBeTrue();
            response2.Error.ShouldBeNull();

            _service.Verify(x => x.Get(config.FlagName, config.UserKey, config.DefaultValue, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
