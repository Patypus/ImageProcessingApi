using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.Images.Dtos;
using Domain.Images.Interfaces;
using ImageProcessingApi.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageProvider;

        public ImageController(IImageService imageProvider)
        {
            _imageProvider = imageProvider;
        }

        /// <summary>
        /// Returns the image as requested by the parameters. 
        /// </summary>
        /// <param name="name">Name of the image to get (no file extension required)</param>
        /// <param name="fileType">The file format to return the image in. Supported types: bmp, png, jpg, gif</param>
        /// <param name="resolution">New DPI resolution to the image to be returned at. This is optiona, passing no value will return the image at its original resolution.</param>
        /// <param name="watermark">Watermark string to apply to the returned image. This is optional and passing nothing results in no watermark being drawn</param>
        /// <param name="backgroundColour">Colour to change the image background to that accepts hex codes and colour names. This is optional and the background is only
        /// changed when this value is populated.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetImage(string name, string fileType, float? resolution = null, string watermark = "", string backgroundColour = "")
        {
            try
            {
                var requestedFileType = ValidateFileType(fileType);
                
                var imageRequest = new ImageRequestDto
                {
                    Name = name,
                    Format = requestedFileType.Format,
                    Resolution = resolution,
                    Watermark = watermark,
                    BackgroundColour = ConvertColourString(backgroundColour)
                };
                
                var image = await _imageProvider.GetImageAsync(imageRequest);
                return File(image, requestedFileType.ContentType);
            }
            catch(ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (FileNotFoundException)
            {
                return NotFound($"Unable to find an image with the name '{name}'");
            }
            catch (Exception exception)
            {
                return Problem(exception.Message, statusCode: (int)HttpStatusCode.InternalServerError);
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

        private Color? ConvertColourString(string colourString)
        {
            return !string.IsNullOrEmpty(colourString)
                ? (Color?)ColorTranslator.FromHtml(colourString)
                : null;
        }
    }
}
