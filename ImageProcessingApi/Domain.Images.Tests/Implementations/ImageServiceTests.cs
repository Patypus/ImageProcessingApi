using Domain.Cache.Abstractions.Dtos;
using Domain.Cache.Abstractions.Proxy;
using Domain.Images.Dtos;
using Domain.Images.FileAccess;
using Domain.Images.Implementations;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Images.Tests.Implementations
{
    [TestFixture]
    public class ImageServiceTests
    {
        [Test]
        public async Task GetImageAsync_PassesCorrectRequestToCacheService()
        {
            var request = new ImageRequestDto
            {
                Name = "fileName",
                Format = ImageFormat.Jpeg,
                Resolution = 124,
                Watermark = "this is a watermark",
                BackgroundColour = Color.Aquamarine
            };

            var mockCahceService = Substitute.For<ICacheService>();
            var mockImage = new byte[8];
            (new Random()).NextBytes(mockImage);
            mockCahceService.GetImageFromCacheAsync(Arg.Any<CacheRequestDto>()).Returns(mockImage);
            var mockImageProvider = Substitute.For<IImageProvider>();

            var service = new ImageService(mockImageProvider, mockCahceService);
            await service.GetImageAsync(request);

            await mockCahceService.Received().GetImageFromCacheAsync(Arg.Is<CacheRequestDto>(arg =>
                arg.Name == request.Name &&
                arg.FileType == request.Format.ToString() &&
                arg.Resolution == request.Resolution.ToString() &&
                arg.Watermark == request.Watermark &&
                arg.BackgroundColour == request.BackgroundColour.ToString()
            ));
        }

        [Test]
        public async Task GetImageAsync_ReturnsImageFromCacheWhenGetReturnsResult()
        {
            var request = new ImageRequestDto
            {
                Name = "fileName",
                Format = ImageFormat.Jpeg
            };

            var mockCahceService = Substitute.For<ICacheService>();
            var mockImage = new byte[8];
            (new Random()).NextBytes(mockImage);
            mockCahceService.GetImageFromCacheAsync(Arg.Any<CacheRequestDto>()).Returns(mockImage);
            var mockImageProvider = Substitute.For<IImageProvider>();

            var service = new ImageService(mockImageProvider, mockCahceService);
            var result = await service.GetImageAsync(request);

            Assert.AreEqual(mockImage, result);
        }

        [Test]
        public async Task GetImageAsync_DoesNotCallImageProviderOrCacheAddWhenCacheHasImageIn()
        {
            var request = new ImageRequestDto
            {
                Name = "fileName",
                Format = ImageFormat.Jpeg,
                Resolution = 124,
                Watermark = "this is a watermark",
                BackgroundColour = Color.Aquamarine
            };

            var mockCahceService = Substitute.For<ICacheService>();
            var mockImage = new byte[8];
            (new Random()).NextBytes(mockImage);
            mockCahceService.GetImageFromCacheAsync(Arg.Any<CacheRequestDto>()).Returns(mockImage);
            var mockImageProvider = Substitute.For<IImageProvider>();

            var service = new ImageService(mockImageProvider, mockCahceService);
            await service.GetImageAsync(request);

            mockImageProvider.Received(0).GetFormattedImage(Arg.Any<ImageRequestDto>());
            await mockCahceService.Received(0).AddImageToCacheAsync(Arg.Any<CacheRequestDto>(), Arg.Any<byte[]>());
        }

        [Test]
        public async Task GetImageAsync_CallsImageProviderWhenNoCachedImageIsReturned()
        {
            var request = new ImageRequestDto
            {
                Name = "fileName",
                Format = ImageFormat.Jpeg,
                Resolution = 124,
                Watermark = "this is a watermark",
                BackgroundColour = Color.Aquamarine
            };

            var mockCahceService = Substitute.For<ICacheService>();
            var mockImageProvider = Substitute.For<IImageProvider>();
            var mockImage = new byte[8];
            (new Random()).NextBytes(mockImage);
            mockImageProvider.GetFormattedImage(request).Returns(mockImage);

            var service = new ImageService(mockImageProvider, mockCahceService);
            var result = await service.GetImageAsync(request);

            Assert.AreEqual(mockImage, result);
        }

        [Test]
        public async Task GetImageAsync_AddsImageFromProviderToCache()
        {
            var request = new ImageRequestDto
            {
                Name = "fileName",
                Format = ImageFormat.Jpeg,
                Resolution = 124,
                Watermark = "this is a watermark",
                BackgroundColour = Color.Aquamarine
            };

            var mockCahceService = Substitute.For<ICacheService>();
            var mockImageProvider = Substitute.For<IImageProvider>();
            var mockImage = new byte[8];
            (new Random()).NextBytes(mockImage);
            mockImageProvider.GetFormattedImage(request).Returns(mockImage);

            var service = new ImageService(mockImageProvider, mockCahceService);
            await service.GetImageAsync(request);

            await mockCahceService.Received().AddImageToCacheAsync(Arg.Is<CacheRequestDto>(arg =>
                arg.Name == request.Name &&
                arg.FileType == request.Format.ToString() &&
                arg.Resolution == request.Resolution.ToString() &&
                arg.Watermark == request.Watermark &&
                arg.BackgroundColour == request.BackgroundColour.ToString()
            ), mockImage);
        }
    }
}
