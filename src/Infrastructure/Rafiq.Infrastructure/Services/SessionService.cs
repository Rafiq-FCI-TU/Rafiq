using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;
namespace Rafiq.Infrastructure.Services
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public SessionService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        #region Create Session

        public async Task<Result<int>> CreateSessionAsync(SessionCreateDto dto)
        {
            string mediaUrl = await UploadFileToAzureAsync(dto.Media, "videos");
            if (dto.Media != null && string.IsNullOrEmpty(mediaUrl))
                return Result.Failure<int>(422, "Failed to upload media. Please try again.");

            string thumbnailUrl = await UploadFileToAzureAsync(dto.Thumbnail, "thumbnails");
            if (dto.Thumbnail != null && string.IsNullOrEmpty(thumbnailUrl))
                return Result.Failure<int>(422, "Failed to upload thumbnail. Please try again.");

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

                return blobClient.Uri.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region Get Sessions

        public async Task<Result<SessionDto>> GetSessionDetailsAsync(int sessionId)
        {
            var session = await _unitOfWork.Repository<Session>().GetEntityAsync(sessionId);
            if (session == null)
                return Result.Failure<SessionDto>(404, "Session not found");

            return Result<SessionDto>.Success(MapToDto(session));
        }

        public async Task<Result<IEnumerable<SessionDto>>> GetSessionsBySpecialistAsync(string specialistId)
        {
            var sessions = await _unitOfWork.Repository<Session>().GetAllAsync();
            ///TODO: Tis filtering should be done at the database level, not in memory. We need to add a method to the repository to filter by specialistId directly in the query. Must implement a spicification pattern for this.
            var filteredSessions = sessions.Where(s => s.SpecialistProfileId == specialistId);

            var dtos = filteredSessions.Select(MapToDto);
            return Result<IEnumerable<SessionDto>>.Success(dtos);
        }

        public async Task<Result<IEnumerable<SessionDto>>> GetSessionsBySpecialistIdForPatientAsync(int patientId)
        {
            var sessions = await _unitOfWork.Repository<Session>().GetAllAsync();
            var dtos = sessions.Select(MapToDto);
            return Result<IEnumerable<SessionDto>>.Success(dtos);
        }

        public async Task<Result<string>> GetSessionVideoUrlAsync(int sessionId, int patientId)
        {
            var session = await _unitOfWork.Repository<Session>().GetEntityAsync(sessionId);
            if (session == null)
                return Result.Failure<string>(404, "Session not found");

            return Result.Success<string>(session.VideoUrl);
        }

        #endregion

        #region Helpers

        private static SessionDto MapToDto(Session session)
        {
            return new SessionDto
            {
                Id = session.Id,
                Title = session.Title,
                Description = session.Description,
                VideoUrl = session.VideoUrl,
                Duration = session.Duration,
                Sequence = session.Sequence,
                Score = session.Score,
                PublishedAt = session.PublishedAt,
                ThumbnailUrl = session.ThumbnailUrl,
                SpecialistProfileId = session.SpecialistProfileId,
                SpecialistName = session.Specialist?.FirstName ?? string.Empty
            };
        }

        #endregion
    }
}