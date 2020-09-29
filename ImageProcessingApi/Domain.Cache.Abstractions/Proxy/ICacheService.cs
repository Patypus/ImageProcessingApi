using Domain.Cache.Abstractions.Dtos;

namespace Domain.Cache.Abstractions.Proxy
{
    public interface ICacheService
    {
        void AddImageToCache(ImageRequestDto requestKey, byte[] imageData);

        void ClearCache();

        byte[] GetImageFromCache(ImageRequestDto requestDto);
    }
}
