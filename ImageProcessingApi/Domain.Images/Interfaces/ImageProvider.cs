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
            using (var image = new Bitmap(Path.Combine(_imageSource.Path, $"{name}.png")))
            {
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
                ColorMap[] remapTable = { colourMap };
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
            };
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
