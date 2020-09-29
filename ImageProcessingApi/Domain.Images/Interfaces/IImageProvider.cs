using System.Drawing.Imaging;

namespace Domain.Images.Interfaces
{
    public interface IImageProvider
    {
        byte[] GetImage(string name, ImageFormat format, string resolution = "", string watermark = "", string backgroundColour = "");
    }
}
