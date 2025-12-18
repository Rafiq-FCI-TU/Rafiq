using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Rafiq.Application.Interfaces.Services;
namespace Rafiq.Infrastructure.Services
{
    internal class CloudinaryService (Cloudinary _cloudinary) : ICloudinaryService
    {
        private readonly Cloudinary cloudinary = _cloudinary;

        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "sessions/videos",
                Type   = "privete"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }
    }
}



