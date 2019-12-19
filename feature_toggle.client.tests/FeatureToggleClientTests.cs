using System;
using Xunit;
using mhlabs.feature_toggle.client.Client;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using mhlabs.feature_toggle.client.Services;
using Microsoft.Extensions.Caching.Memory;

namespace mhlabs.feature_toggle.client.tests
{
    public class FeatureToggleClientTests
    {
        private readonly FeatureToggleClient _client;
        private readonly Mock<IFeatureToggleService> _service = new Mock<IFeatureToggleService>();
        private readonly Mock<IFeatureToggleConfiguration> _configuration = new Mock<IFeatureToggleConfiguration>();
        private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        
        public FeatureToggleClientTests()
        {
            _client = new FeatureToggleClient(_service.Object, _configuration.Object, _cache, NullLogger<FeatureToggleClient>.Instance);
        }

        [Fact]
        public void Should_Return_Response_When_Succesful()
        {

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
}
