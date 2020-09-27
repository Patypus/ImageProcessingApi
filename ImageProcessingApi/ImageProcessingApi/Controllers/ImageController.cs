using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Images;
using Domain.Images.Implementations;
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
        public IActionResult Image(string name, string resolution, string fileType, string watermark = "", string backgroundColour = "")
        {
            var i = _imageProvider.GetImage(name, resolution, fileType, watermark, backgroundColour);
            return File(i, "image/jpeg");
        }
    }
}
