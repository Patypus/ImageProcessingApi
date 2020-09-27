using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessingApi.Configuration
{
    public class ImageTypeConfiguration
    {
        public ImageFormat Format { get; set; }
        public string ContentType { get; set; }
    }
}
