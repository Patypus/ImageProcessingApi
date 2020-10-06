using Domain.Cache.Abstractions.Proxy;
using ImageProcessingApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ImageProcessingApi.Tests.Controllers
{
    [TestFixture]
    public class CacheControllerTests : ControllerTestsBase<CacheController>
    {

        [Test]
        public async Task ClearCacheAsync_CallsCacheService()
        {
            var mockCacheService = Substitute.For<ICacheService>();
            _controller = new CacheController(mockCacheService);
            
            await _controller.ClearCacheAsync();

            await mockCacheService.Received(1).ClearCacheAsync();
        }

        [Test]
        public async Task ClearCacheAsync_ReportsExceptionReturnedFromService()
        {
            var exceptionMessage = "error";
            var mockCacheService = Substitute.For<ICacheService>();
            mockCacheService.ClearCacheAsync().Throws(new Exception(exceptionMessage));
            _controller = new CacheController(mockCacheService);

            SetupContext();

            var result = await _controller.ClearCacheAsync();
            var castResult = (ObjectResult)result;

            Assert.NotNull(castResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, castResult.StatusCode);
            Assert.AreEqual(exceptionMessage, castResult.Value);
        }
    }
}
