using Domain.Images.Configuration;
using Domain.Images.Implementations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Domain.Images.Interfaces
{
    public class ImageProvider : IImageProvider
    {
        private readonly ImageSource _imageSource;

        public ImageProvider(IOptions<ImageSource> imageSoruce)
        {
            _imageSource = imageSoruce.Value;
        }

        public byte[] GetImage(string name, ImageFormat format, string resolution = "", string watermark = "", string backgroundColour = "")
        {
            using (var impage = new Bitmap(Path.Combine(_imageSource.Path, $"{name}.png")))
            {
                using (var stream = new MemoryStream())
                {
                    impage.Save(stream, format);
                    return stream.ToArray();
                }
            };
        }
    }
}
