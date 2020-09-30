using Domain.Cache.Abstractions.Dtos;
using Domain.Cache.Abstractions.Proxy;
using Domain.Images.Configuration;
using Domain.Images.Filestystem;
using Domain.Images.Implementations;
using Domain.Images.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Domain.Images.Implementations
{
    public class ImageProvider : IImageProvider
    {
        private readonly ImageSource _imageSource;
        private readonly ICacheService _cacheService;
        private readonly IFileService _fileService;

        public ImageProvider(IOptions<ImageSource> imageSoruce, ICacheService cacheService, IFileService fileService)
        {
            _imageSource = imageSoruce.Value;
            _cacheService = cacheService;
            _fileService = fileService;
        }

        public byte[] GetImage(string name, ImageFormat format, string resolution = "", string watermark = "", string backgroundColour = "")
        {
            var cacheRequest = new ImageRequestDto
            {
                Name = name
            };

            var image = _cacheService.GetImageFromCache(cacheRequest);
            if (image == null || image.Length == 0)
            {
                image = GetImageFromDisk(name, format, resolution, watermark, backgroundColour);
                _cacheService.AddImageToCache(cacheRequest, image);
            }
            return image;
        }

        private byte[] GetImageFromDisk(string name, ImageFormat format, string resolution = "", string watermark = "", string backgroundColour = "")
        {
            var image = _fileService.LoadImage(_imageSource.Path, name);
            //using (var image = new Bitmap(Path.Combine(_imageSource.Path, $"{name}.png")))
            //{
                if (!string.IsNullOrEmpty(watermark))
                {
                    ApplyWaterMark(image, watermark);
                }

                //recolour
                var colourMap = new ColorMap
                {
                    OldColor = image.GetPixel(0, 0),
                    NewColor = Color.Chartreuse
                };
                var colourMap2 = new ColorMap
                {
                    OldColor = image.GetPixel(20, 20),
                    NewColor = Color.Chartreuse
                };
                ColorMap[] remapTable = { colourMap, colourMap2 };
                var attributes = new ImageAttributes();
                attributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                var newBmp = new Bitmap(image.Width, image.Height);
                using (var gfx = Graphics.FromImage(newBmp))
                {
                    var rectangle = new Rectangle(0, 0, image.Width, image.Height);
                    gfx.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

                    using (var stream = new MemoryStream())
                    {
                        newBmp.Save(stream, format);
                        return stream.ToArray();
                    }
                }

                //using (var stream = new MemoryStream())
                //{
                //    image.Save(stream, format);
                //    return stream.ToArray();
                //}
            //};
        }

        private void ApplyWaterMark(Image image, string watermark)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                using (var font = new Font("consolas", 10))
                {
                    var stringSize = graphics.MeasureString(watermark, font);
                    var watermarkLocation = CalculateWatermarkLocation(image.Height, image.Width, stringSize);
                    
                    graphics.DrawString(watermark, font, Brushes.LightGray, new Point(watermarkLocation.x, watermarkLocation.y));
                }
            }
        }

        private (int x, int y) CalculateWatermarkLocation(int imageHeight, int imageWidth, SizeF watermarkTextSize)
        {
            var halfImageHeight = imageHeight / 2;
            var halfImageWidth = imageWidth / 2;
            var watermarkX = halfImageWidth - (watermarkTextSize.Width / 2);
            var watermarkY = halfImageHeight - (watermarkTextSize.Height / 2);

            return (x: (int)Math.Max(0, watermarkX), y: (int)Math.Max(0, watermarkY));
        }
    }
}
