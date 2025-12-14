using Microsoft.AspNetCore.Http;

namespace Rafiq.Application.Interfaces.Services
{
    public interface ICloudinaryService
    {
        // Uploads an image file to Cloudinary and returns the URL of the uploaded image.
        // And this URL for rlated to myplatform not cloudinary directly 
        Task<string> UploadVideoAsync(IFormFile file);
    }
}