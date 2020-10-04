using Domain.Images.Dtos;

namespace Domain.Images.FileAccess
{
    public interface IImageProvider
    {
        /// <summary>
        /// Return an image from disk with the formatting specified in the <paramref name="imageRequest"/> parameter
        /// applied to it
        /// </summary>
        /// <param name="imageRequest">Dto containing the details of the image to get</param>
        /// <returns>Byte array of image data for the image requested.</returns>
        byte[] GetFormattedImage(ImageRequestDto imageRequest);
    }
}
