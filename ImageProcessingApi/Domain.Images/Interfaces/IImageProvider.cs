using Domain.Images.Dtos;
using System.Threading.Tasks;

namespace Domain.Images.Interfaces
{
    public interface IImageProvider
    {
        /// <summary>
        /// Returns an image as a byte array that is formatted to the requirements of the <paramref name="request"/> parameter
        /// </summary>
        /// <param name="request">Dto containing the details of the image to return</param>
        /// <returns>byre array of the image data</returns>
        Task<byte[]> GetImageAsync(ImageRequestDto request);
    }
}
