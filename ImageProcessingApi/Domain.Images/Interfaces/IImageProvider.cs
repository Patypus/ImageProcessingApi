using Domain.Images.Dtos;
using System.Threading.Tasks;

namespace Domain.Images.Interfaces
{
    public interface IImageProvider
    {
        Task<byte[]> GetImageAsync(ImageRequestDto request);
    }
}
