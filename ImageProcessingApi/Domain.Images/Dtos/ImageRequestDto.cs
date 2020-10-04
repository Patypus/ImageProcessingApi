using System.Drawing;
using System.Drawing.Imaging;

namespace Domain.Images.Dtos
{
    public class ImageRequestDto
    {
        public string Name { get; set; }
        public ImageFormat Format { get; set; }
        public float? Resolution { get; set; }
        public string Watermark { get; set; }
        public Color? BackgroundColour {get;set;}
    }
}
