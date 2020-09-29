using Domain.Images.Implementations;
using Domain.Images.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Images
{
    public static class DomainImagesSetup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IImageProvider, ImageProvider>();
        }
    }
}
