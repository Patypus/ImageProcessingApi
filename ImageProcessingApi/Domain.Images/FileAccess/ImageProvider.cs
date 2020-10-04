using Domain.Images.Configuration;
using Domain.Images.Dtos;
using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Domain.Images.FileAccess
{
    public class ImageProvider : IImageProvider
    {
        private readonly ImageSource _imageSource;

        public ImageProvider(IOptions<ImageSource> imageSource)
        {
            _imageSource = imageSource.Value;
        }

        public byte[] GetFormattedImage(ImageRequestDto request)
        {
            using (var fileStream = GetImageStream(Path.Combine(_imageSource.Path, $"{request.Name}.{_imageSource.DefaultImageFileType}")))
            {
                var image = new Bitmap(fileStream);

                if (!string.IsNullOrEmpty(request.Watermark))
                {
                    ApplyWaterMark(image, request.Watermark);
                }

                if (request.BackgroundColour.HasValue)
                {
                    image = ColourImageBackground(image, request.BackgroundColour.Value);
                }

                if (request.Resolution.HasValue)
                {
                    image.SetResolution(request.Resolution.Value, request.Resolution.Value);
                }

                return GetImageBytes(image, request.Format);
            }
        }

        private FileStream GetImageStream(string imagePath)
        {
            return new FileStream(imagePath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read);
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

        private Bitmap ColourImageBackground(Image image, Color backgroundColour)
        {
            var attributes = new ImageAttributes();

            var newBmp = new Bitmap(image.Width, image.Height);
            using (var gfx = Graphics.FromImage(newBmp))
            {
                var rectangle = new Rectangle(0, 0, image.Width, image.Height);
                gfx.FillRectangle(new SolidBrush(backgroundColour), rectangle);
                gfx.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            return newBmp;
        }

        private byte[] GetImageBytes(Image image, ImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, format);
                return stream.ToArray();
            }
        }
    }
}
