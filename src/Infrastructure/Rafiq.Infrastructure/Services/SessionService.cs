using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;
using Rafiq.Infrastructure.Settings;

namespace Rafiq.Infrastructure.Services;
public class SessionService : ISessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IMapper _mapper;
    private readonly AzureBlobSettings azureBlobSettings;
    private readonly string _containerName;

    public SessionService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper , AzureBlobSettings azureBlobSettings)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _blobServiceClient = new BlobServiceClient(azureBlobSettings.ConnectionString);
        _containerName = azureBlobSettings.ContainerName;
    }

    #region Create Session

    public async Task<Result<int>> CreateSessionAsync(SessionCreateDto dto)
    {
        try
        {
            var mediaUrl = await UploadFileAsync(dto.Media, "videos");
            var thumbnailUrl = await UploadFileAsync(dto.Thumbnail, "thumbnails");
            var duration =  GetVideoDuration(dto.Media);

            var session = new Session
            {
                Title = dto.Title,
                Description = dto.Description,
                VideoUrl = mediaUrl,
                Duration = duration,
                Sequence = 0,
                Score = dto.Score,
                Notes= dto.Notes,
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

    //public async Task<Result<int>> SubmitSessionForPatientReviewAsync(SessionCreateReviewPatientDto )
    //{
    //    new NotImplementedException();
    //}

    #endregion

    #region Get Sessions

    public async Task<Result<SessionDto>> GetSessionDetailsAsync(int sessionId)
    {
        try
        {
            var spec = new BaseSpecifications<Session>
            {
                WhereCriteria = s => s.Id == sessionId
            };

            spec.AddInclude("Specialist");

            var session = await _unitOfWork.Repository<Session>().GetEntityAsync(spec);
            if (session == null)
                return Result.Failure<SessionDto>(404, "Session not found");

            return Result.Success(_mapper.Map<SessionDto>(session));
        }
        catch (Exception ex)
        {
            return Result.Failure<SessionDto>(500, $"Failed to get session details: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<SessionsBySpecialistDto>>> GetSessionsBySpecialistAsync(string specialistId)
    {
        try
        {
            var spec = new BaseSpecifications<Session>
            {
                WhereCriteria = s => s.SpecialistProfileId == specialistId
            };

            spec.AddInclude("Specialist");

            var sessions = await _unitOfWork.Repository<Session>().GetAllAsync(spec);
            var dtos = _mapper.Map<IEnumerable<SessionsBySpecialistDto>>(sessions);

            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<SessionsBySpecialistDto>>(500, $"Failed to get sessions: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Sessionfor_Specialist_patientDto>>> GetSessionsByPatientAndStatusAsync(int patientId, string status)
    {
        try
        {
            var patientSpec = new BaseSpecifications<Patient>
            {
                WhereCriteria = p => p.Id == patientId
            };
            patientSpec.AddIncludeWithThenInclude(p => p.SpecialistPatients, "Specialist");

            var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientSpec);
            if (patient == null)
                return Result.Failure<IEnumerable<Sessionfor_Specialist_patientDto>>(404, "Patient not found");

            var specialist = patient.SpecialistPatients.FirstOrDefault();
            if (specialist == null)
                return Result.Failure<IEnumerable<Sessionfor_Specialist_patientDto>>(404, "No sessions found. Please select a specialist first.");

            var sessionSpec = new BaseSpecifications<Session>
            {
                WhereCriteria = s => s.SpecialistProfileId == specialist.SpecialistId
            };
            sessionSpec.AddInclude("Specialist");

            var sessions = await _unitOfWork.Repository<Session>().GetAllAsync(sessionSpec);

            var filteredSessions = FilterSessionsByStatus(sessions, patient.TotalScore, status).ToList();
            if (!filteredSessions.Any())
                return Result.Failure<IEnumerable<Sessionfor_Specialist_patientDto>>(404, "No sessions found");

            var dtos = _mapper.Map<IEnumerable<Sessionfor_Specialist_patientDto>>(
                filteredSessions,
                opt => {
                    opt.Items["PatientId"] = patientId;
                    opt.Items["PatientName"] = patient.FirstName;
                }
            );

            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<Sessionfor_Specialist_patientDto>>(500, $"Failed to get sessions: {ex.Message}");
        }
    }
    #endregion

    #region Helper Methods

    private async Task<string> UploadFileAsync(IFormFile? file, string folder)
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


    private TimeSpan GetVideoDuration(IFormFile video)
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
    private IEnumerable<Session> FilterSessionsByStatus(IReadOnlyList<Session> sessions, double totalScore, string status)
    {
        return status.ToLower() switch
        {
            "allowed" => sessions.Where(s => s.Score <= totalScore),
            "not-allowed" => sessions.Where(s => s.Score > totalScore),
            _ => []
        };
    }



    #endregion
}
