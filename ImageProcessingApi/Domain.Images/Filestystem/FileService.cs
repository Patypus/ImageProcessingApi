using System.Drawing;
using System.IO;

namespace Domain.Images.Filestystem
{
    public class FileService : IFileService
    {
        private readonly object lockObject = new object();

        public Bitmap LoadImage(string path, string name)
        {
            Bitmap result;

            lock (lockObject)
            {
                using (var stream = new FileStream(Path.Combine(path, $"{name}.png"), FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    result = new Bitmap(stream);
                }
            }

            return result;
        }
    }
}
