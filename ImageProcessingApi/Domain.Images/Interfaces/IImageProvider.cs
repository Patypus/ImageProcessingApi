using Domain.Images.Dtos;

namespace Domain.Images.Interfaces
{
    public interface IImageProvider
    {
        byte[] GetImage(ImageRequestDto request);
    }
}
