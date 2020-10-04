using Domain.Cache.Abstractions.Dtos;
using Domain.Cache.Abstractions.Proxy;
using Domain.Images.Dtos;
using Domain.Images.FileAccess;
using Domain.Images.Interfaces;
using System.Threading.Tasks;

namespace Domain.Images.Implementations
{
    public class ImageService : IImageService
    {
        
        private readonly ICacheService _cacheService;
        private readonly IImageProvider _imageProvider;

        public ImageService(IImageProvider imageProvider, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _imageProvider = imageProvider;
        }

        public async Task<byte[]> GetImageAsync(ImageRequestDto request)
        {
            var cacheRequest = new CacheRequestDto
            {
                Name = request.Name,
                FileType = request.Format.ToString(),
                Resolution = request.Resolution.HasValue 
                    ? request.Resolution.Value.ToString()
                    : string.Empty,
                Watermark = request.Watermark,
                BackgroundColour = request.BackgroundColour.HasValue ? request.BackgroundColour.Value.ToString() : string.Empty
            };

            var image = await _cacheService.GetImageFromCacheAsync(cacheRequest);
            if (image == null || image.Length == 0)
            {
                image = _imageProvider.GetFormattedImage(request);
                await _cacheService.AddImageToCacheAsync(cacheRequest, image);
            }
            return image;
        }
    }
}
