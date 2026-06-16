using Rafiq.Application.Dtos.ResourceDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;

public interface IResourceService
{
    Task<Result<ResourceResponseDto>> CreateResourceAsync(string userId, CreateResourceDto dto);
    Task<Result<IReadOnlyList<ResourceResponseDto>>> GetAllResourcesAsync();
    Task<Result<ResourceResponseDto>> GetResourceByIdAsync(int id);
    Task<Result<ResourceResponseDto>> UpdateResourceAsync(int id, string userId, UpdateResourceDto dto);
    Task<Result> DeleteResourceAsync(int id, string userId);
}
