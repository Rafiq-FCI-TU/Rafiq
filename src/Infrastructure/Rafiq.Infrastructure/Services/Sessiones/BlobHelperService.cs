using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Http;
using Rafiq.Infrastructure.Settings;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Rafiq.Infrastructure.Services.Sessiones;

public class BlobHelperService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobHelperService(AzureBlobSettings azureBlobSettings)
    {
        _blobServiceClient = new BlobServiceClient(azureBlobSettings.ConnectionString);
        _containerName = azureBlobSettings.ContainerName;
    }

    public async Task<string> UploadFileAsync(IFormFile? file, string folder)
    {
        if (file == null) return string.Empty;

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobName = $"{folder}/{Guid.NewGuid()}_{file.FileName}";
        var blobClient = containerClient.GetBlobClient(blobName);

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerClient.Name,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddYears(1)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        return blobClient.GenerateSasUri(sasBuilder).ToString();
    }


    public TimeSpan GetVideoDuration(IFormFile video)
    {
        var tempFile = Path.GetTempFileName();

        using (var stream = File.Create(tempFile))
        {
            video.CopyTo(stream);
        }

        var inputFile = new MediaFile { Filename = tempFile };
        using var engine = new Engine();
        engine.GetMetadata(inputFile);

        File.Delete(tempFile);
        return inputFile.Metadata.Duration;
    }




}