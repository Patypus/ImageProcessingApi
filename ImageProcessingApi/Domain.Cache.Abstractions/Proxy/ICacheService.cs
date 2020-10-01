using Domain.Cache.Abstractions.Dtos;
using System.Threading.Tasks;

namespace Domain.Cache.Abstractions.Proxy
{
    public interface ICacheService
    {
        Task AddImageToCacheAsync(CacheRequestDto requestKey, byte[] imageData);

        Task ClearCacheAsync();

        Task<byte[]> GetImageFromCacheAsync(CacheRequestDto requestDto);
    }
}
