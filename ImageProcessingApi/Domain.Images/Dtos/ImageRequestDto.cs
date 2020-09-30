using System.Drawing.Imaging;

namespace Domain.Images.Dtos
{
    public class ImageRequestDto
    {
        public string Name { get; set; }
        public ImageFormat Format { get; set; }
        public float? ResolutionX { get; set; }
        public float? ResolutionY { get; set; }
        public string Watermark { get; set; }
        public string BackgroundColour {get;set;}
    }
}
