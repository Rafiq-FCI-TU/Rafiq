using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        #region Create Session

        public async Task<Result<int>> CreateSessionAsync(SessionCreateDto dto)
        {
            string mediaUrl;
            try
            {
                mediaUrl = await UploadFileToAzureAsync(dto.Media, "videos");
                if (dto.Media != null && string.IsNullOrEmpty(mediaUrl))
                    return Result.Failure<int>(422, "Failed to upload media. Please try again.");
            }
            catch (Exception ex)
            {
                return Result.Failure<int>(500, $"Media upload failed: {ex.Message}");
            }

            string thumbnailUrl;
            try
            {
                thumbnailUrl = await UploadFileToAzureAsync(dto.Thumbnail, "thumbnails");
                if (dto.Thumbnail != null && string.IsNullOrEmpty(thumbnailUrl))
                    return Result.Failure<int>(422, "Failed to upload thumbnail. Please try again.");
            }
            catch (Exception ex)
            {
                return Result.Failure<int>(500, $"Thumbnail upload failed: {ex.Message}");
            }

            try
            {
                var session = new Session
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    VideoUrl = mediaUrl,
                    Duration = dto.Duration,
                    Sequence = dto.Sequence,
                    Score = dto.Score,
                    PublishedAt = dto.PublishedAt,
                    ThumbnailUrl = thumbnailUrl,
                    SpecialistProfileId = dto.SpecialistProfileId
                };

                await _unitOfWork.Repository<Session>().AddAsync(session);
                await _unitOfWork.CompleteAsync();

                return Result.Success(session.Id);
            }
            catch (Exception ex)
            {
                return Result.Failure<int>(500, $"Failed to create session: {ex.Message}");
            }
        }

        //private async Task<string> UploadFileToAzureAsync(IFormFile? file, string folder)
        //{
        //    if (file == null) return string.Empty;

        //    try
        //    {
        //        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        //        await containerClient.CreateIfNotExistsAsync();

        //        string blobName = $"{folder}/{Guid.NewGuid()}_{file.FileName}";
        //        var blobClient = containerClient.GetBlobClient(blobName);

        //        using var stream = file.OpenReadStream();
        //        await blobClient.UploadAsync(stream, overwrite: true);

        //        return blobClient.Uri.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Azure upload failed: {ex.Message}");
        //    }
        //}
        private async Task<string> UploadFileToAzureAsync(IFormFile? file, string folder)
        {
            if (file == null) return string.Empty;

            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync();

                string blobName = $"{folder}/{Guid.NewGuid()}_{file.FileName}";
                var blobClient = containerClient.GetBlobClient(blobName);

                using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, overwrite: true);

                //  Generate SAS URL (valid for 1 hour)
                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobName,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                var sasUri = blobClient.GenerateSasUri(sasBuilder);

                return sasUri.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Azure upload failed: {ex.Message}");
            }
        }

        #endregion

        #region Get Sessions

        public async Task<Result<SessionDto>> GetSessionDetailsAsync(int sessionId)
        {
            try
            {
                var session = await _unitOfWork.Repository<Session>().GetEntityAsync(sessionId);
                if (session == null)
                    return Result.Failure<SessionDto>(404, "Session not found");

                return Result<SessionDto>.Success(_mapper.Map<SessionDto>(session));
            }
            catch (Exception ex)
            {
                return Result.Failure<SessionDto>(500, $"Failed to get session details: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<SessionDto>>> GetSessionsBySpecialistAsync(string specialistId)
        {
            try
            {
                var sessions = await _unitOfWork.Repository<Session>().GetAllAsync();
                var filteredSessions = sessions.Where(s => s.SpecialistProfileId == specialistId);
                var dtos = _mapper.Map<IEnumerable<SessionDto>>(filteredSessions);
                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<SessionDto>>(500, $"Failed to get sessions: {ex.Message}");
            }
        }
        public async Task<Result<IEnumerable<SessionDto>>> GetSessionsBySpecialistIdForPatientAsync(string specialistId, int patientId)
        {
            try
            {
                // Get all sessions for that specialist
                var sessions = await _unitOfWork.Repository<Session>().GetAllAsync();

                /// TODO: create spafification pattern for this filtering logic and move it to repository layer 
                var filteredSessions = sessions
                    .Where(s => s.SpecialistProfileId == specialistId
                                && s.Specialist.SpecialistPatients.Any(sp => sp.PatientId == patientId))
                    .ToList();

                var dtos = _mapper.Map<IEnumerable<SessionDto>>(filteredSessions);
                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<SessionDto>>(500, $"Failed to get sessions for patient: {ex.Message}");
            }
        }
        public async Task<Result<string>> GetSessionVideoUrlAsync(int sessionId, int patientId)
        {
            try
            {
                var session = await _unitOfWork.Repository<Session>().GetEntityAsync(sessionId);
                if (session == null)
                    return Result.Failure<string>(404, "Session not found");

                return Result.Success<string>(session.VideoUrl);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(500, $"Failed to get video URL: {ex.Message}");
            }
        }

        #endregion
    }
}