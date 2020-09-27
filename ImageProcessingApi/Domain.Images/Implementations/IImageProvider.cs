using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Images.Implementations
{
    public interface IImageProvider
    {
        byte[] GetImage(string name, string resolution, string fileType, string watermark = "", string backgroundColour = "");
    }
}
