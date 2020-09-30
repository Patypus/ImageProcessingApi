using System.Drawing;

namespace Domain.Images.Filestystem
{
    public interface IFileService
    {
        Bitmap LoadImage(string path, string name);
    }
}
