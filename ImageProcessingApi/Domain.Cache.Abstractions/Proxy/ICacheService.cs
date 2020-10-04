using Domain.Cache.Abstractions.Dtos;
using System.Threading.Tasks;

namespace Domain.Cache.Abstractions.Proxy
{
    public interface ICacheService
    {
        /// <summary>
        /// Adds an image into the cache. The details in the <paramref name="requestKey"/> parameter are formatted to generate the 
        /// cache key to store the <paramref name="imageData"/> against.
        /// </summary>
        /// <param name="requestKey">Dto encapsulating the identifing information of the image</param>
        /// <param name="imageData">byte array of image data to be cached</param>
        /// <returns></returns>
        Task AddImageToCacheAsync(CacheRequestDto requestKey, byte[] imageData);

        /// <summary>
        /// Clears the cache of stored data
        /// </summary>
        /// <returns></returns>
        Task ClearCacheAsync();

        /// <summary>
        /// Retrieve and image from the cache, using the details from <paramref name="requestKey"/> param to identify the
        /// image. If no image is in the cache corresponding to the passed details, null will be returned.
        /// </summary>
        /// <param name="requestKey">Dto encapsulating the identifing information of the image</param>
        /// <returns>Byte array of image data for the image that matches the request, or null if no cached image matches</returns>
        Task<byte[]> GetImageFromCacheAsync(CacheRequestDto requestKey);
    }
}
