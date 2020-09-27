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

        public byte[] GetImage(string name, string resolution, string fileType, string watermark = "", string backgroundColour = "")
        {
            using (var t = new Bitmap(Path.Combine(_imageSource.Path, $"{name}.png")))
            {
                using (var ms = new MemoryStream())
                {
                    t.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            };
        }
    }
}
