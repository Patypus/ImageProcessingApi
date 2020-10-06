using Domain.Cache.Abstractions.Proxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ImageProcessingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// This endpoint will clear the contents of the cache
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> ClearCacheAsync()
        {
            try
            {
                await _cacheService.ClearCacheAsync();
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}
