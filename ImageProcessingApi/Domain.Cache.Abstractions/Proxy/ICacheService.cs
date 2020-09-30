using Domain.Cache.Abstractions.Dtos;

namespace Domain.Cache.Abstractions.Proxy
{
    public interface ICacheService
    {
        void AddImageToCache(CacheRequestDto requestKey, byte[] imageData);

        void ClearCache();

        byte[] GetImageFromCache(CacheRequestDto requestDto);
    }
}
