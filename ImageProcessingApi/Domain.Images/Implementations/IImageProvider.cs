using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace Domain.Images.Implementations
{
    public interface IImageProvider
    {
        byte[] GetImage(string name, ImageFormat format, string resolution = "", string watermark = "", string backgroundColour = "");
    }
}
