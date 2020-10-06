using Domain.Images.Dtos;
using Domain.Images.Interfaces;
using ImageProcessingApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingApi.Tests.Controllers
{
    [TestFixture]
    public class ImageControllerTests : ControllerTestsBase<ImageController>
    {
        [Test]
        public async Task GetImage_PassesParametersToService()
        {
            var mockImageService = Substitute.For<IImageService>();
            _controller = new ImageController(mockImageService);

            var name = "imageName";
            var fileType = "jpeg";
            var resolution = 36;
            var watermark = "watermark string";
            var colour = "Red";

            await _controller.GetImage(name, fileType, resolution, watermark, colour);

            await mockImageService.Received().GetImageAsync(Arg.Is<ImageRequestDto>(arg =>
                arg.Name == name &&
                arg.Format == ImageFormat.Jpeg &&
                arg.Resolution == resolution &&
                arg.Watermark == watermark &&
                arg.BackgroundColour.Value.Name == colour
            ));
        }

        [Test]
        public async Task GetImage_ReturnsImageDataFromService()
        {
            var mockImageService = Substitute.For<IImageService>();
            var mockImageData = new byte[8];
            (new Random()).NextBytes(mockImageData);
            mockImageService.GetImageAsync(Arg.Any<ImageRequestDto>()).Returns(mockImageData);
            _controller = new ImageController(mockImageService);

            var result = await _controller.GetImage("name", "jpeg");
            var castResult = (FileContentResult)result;

            Assert.AreEqual(mockImageData, castResult.FileContents);
        }

        [Test]
        public void GetImage_SetsImageContent()
        {
            var mockImageService = Substitute.For<IImageService>();
            _controller = new ImageController(mockImageService);

            var supportedTypes = new List<(string name, string format)>
            {
                (name: "bmp", format: "image/bmp"),
                (name: "png", format: "image/png"),
                (name: "jpeg", format: "image/jpeg"),
                (name: "jpg", format: "image/jpeg"),
                (name: "gif", format: "image/gif")
            };

            supportedTypes.ForEach(async typeDetails =>
            {
                var result = await _controller.GetImage("name", typeDetails.name);
                var castResult = (FileContentResult)result;

                Assert.AreEqual(typeDetails.format, castResult.ContentType);
            });
        }

        [Test]
        public async Task GetImage_ReturnsBadRequestForUnSupportedFileType()
        {
            var mockImageService = Substitute.For<IImageService>();
            _controller = new ImageController(mockImageService);
            var unsupportedType = "docx";

            var result = await _controller.GetImage("name", unsupportedType);
            var castResult = (BadRequestObjectResult)result;

            Assert.NotNull(castResult);
            Assert.AreEqual($"{unsupportedType} is not a supported image type. Supported types are: Bmp, Jpeg, Png, Gif", castResult.Value.ToString());
        }

        [Test]
        public async Task GetImage_ReturnsNotFoundForFileNotFoundException()
        {
            var fileName = "the_file_I_want";
            var mockImageService = Substitute.For<IImageService>();
            mockImageService.GetImageAsync(Arg.Any<ImageRequestDto>())
                .Throws(new FileNotFoundException("file not found"));
            _controller = new ImageController(mockImageService);
            SetupContext();

            var result = await _controller.GetImage(fileName, "jpg");
            var castResult = (NotFoundObjectResult)result;

            Assert.NotNull(castResult);
            Assert.AreEqual($"Unable to find an image with the name '{fileName}'", castResult.Value.ToString());
        }

        [Test]
        public async Task GetImage_ReturnsErrorForGenericExceptionFromService()
        {
            var exceptionMessage = "its all gone wrong";
            var mockImageService = Substitute.For<IImageService>();
            mockImageService.GetImageAsync(Arg.Any<ImageRequestDto>()).Throws(new Exception(exceptionMessage));
            _controller = new ImageController(mockImageService);
            SetupContext();

            var result = await _controller.GetImage("name", "jpg");
            var castResult = (ObjectResult)result;

            Assert.NotNull(castResult);
            Assert.AreEqual(exceptionMessage, castResult.Value.ToString());
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, castResult.StatusCode);
        }
    }
}
