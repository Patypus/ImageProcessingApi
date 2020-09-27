using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain.Images;
using Domain.Images.Implementations;
using ImageProcessingApi.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageProvider _imageProvider;

        public ImageController(IImageProvider imageProvider)
        {
            _imageProvider = imageProvider;
        }

        [HttpGet]
        public IActionResult Image(string name, string fileType, string resolution, string watermark = "", string backgroundColour = "")
        {
            try
            {
                var requestedFileType = ValidateFileType(fileType);

                var image = _imageProvider.GetImage(name, requestedFileType.Format, resolution, watermark, backgroundColour);
                return File(image, requestedFileType.ContentType);
            }
            catch(ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        private ImageTypeConfiguration ValidateFileType(string requestedFileType)
        {
            switch (requestedFileType.ToLower())
            {
                case "bmp":
                    return new ImageTypeConfiguration { Format = ImageFormat.Bmp, ContentType = "image/bmp" };
                case "png":
                    return new ImageTypeConfiguration { Format = ImageFormat.Png, ContentType = "image/png" };
                case "jpeg":
                case "jpg":
                    return new ImageTypeConfiguration { Format = ImageFormat.Jpeg, ContentType = "image/jpeg" };
                case "gif":
                    return new ImageTypeConfiguration { Format = ImageFormat.Gif, ContentType = "image/gif" };
                default:
                    throw new ArgumentException($"{requestedFileType} is not a supported image type. Supported types are: Bmp, Jpeg, Png, Gif");
            };
        }
    }
}
