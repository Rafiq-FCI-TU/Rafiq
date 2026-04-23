using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Dtos.ResourceDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities.Resources;
using Rafiq.Domain.ErrorHandling;
using Rafiq.Persistence.Data;

namespace Rafiq.Infrastructure.Services;

public class ResourceService : IResourceService
{
    private readonly RafiqDbContext _context;

    public ResourceService(RafiqDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ResourceResponseDto>> CreateResourceAsync(string userId, CreateResourceDto dto)
    {
        try
        {
            var resource = new Resource
            {
                Title = dto.Title.Trim(),
                Link = dto.Link.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow,
                Tags = NormalizeTags(dto.Tags)
                    .Select(tag => new ResourceTag { Value = tag })
                    .ToList()
            };

            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            var created = await GetResourcesQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(resourceItem => resourceItem.Id == resource.Id);

            if (created is null)
                return Result.Failure<ResourceResponseDto>(500, "Resource was created but could not be reloaded.");

            return Result.Success(MapToResponseDto(created), "Resource created successfully.");
        }
        catch (DbUpdateException)
        {
            return Result.Failure<ResourceResponseDto>(400, "Unable to save resource. Please make sure tags are unique.");
        }
        catch (Exception ex)
        {
            return Result.Failure<ResourceResponseDto>(500, $"Failed to create resource: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyList<ResourceResponseDto>>> GetAllResourcesAsync()
    {
        try
        {
            var resources = await GetResourcesQuery()
                .AsNoTracking()
                .OrderByDescending(resourceItem => resourceItem.CreatedAt)
                .ToListAsync();

            var response = resources
                .Select(MapToResponseDto)
                .ToList();

            return Result.Success<IReadOnlyList<ResourceResponseDto>>(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<ResourceResponseDto>>(500, $"Failed to fetch resources: {ex.Message}");
        }
    }

    public async Task<Result<ResourceResponseDto>> GetResourceByIdAsync(int id)
    {
        try
        {
            var resource = await GetResourcesQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(resourceItem => resourceItem.Id == id);

            if (resource is null)
                return Result.Failure<ResourceResponseDto>(404, "Resource not found.");

            return Result.Success(MapToResponseDto(resource));
        }
        catch (Exception ex)
        {
            return Result.Failure<ResourceResponseDto>(500, $"Failed to fetch resource: {ex.Message}");
        }
    }

    public async Task<Result<ResourceResponseDto>> UpdateResourceAsync(int id, string userId, UpdateResourceDto dto)
    {
        try
        {
            var resource = await _context.Resources
                .Include(resourceItem => resourceItem.Tags)
                .FirstOrDefaultAsync(resourceItem => resourceItem.Id == id);

            if (resource is null)
                return Result.Failure<ResourceResponseDto>(404, "Resource not found.");

            if (resource.AppUserId != userId)
                return Result.Failure<ResourceResponseDto>(403, "You are not authorized to update this resource.");

            resource.Title = dto.Title.Trim();
            resource.Link = dto.Link.Trim();
            resource.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();

            if (resource.Tags.Count > 0)
                _context.ResourceTags.RemoveRange(resource.Tags);

            var normalizedTags = NormalizeTags(dto.Tags);
            if (normalizedTags.Count > 0)
            {
                var newTags = normalizedTags.Select(tag => new ResourceTag
                {
                    ResourceId = resource.Id,
                    Value = tag
                });

                await _context.ResourceTags.AddRangeAsync(newTags);
            }

            await _context.SaveChangesAsync();

            var updated = await GetResourcesQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(resourceItem => resourceItem.Id == id);

            if (updated is null)
                return Result.Failure<ResourceResponseDto>(500, "Resource was updated but could not be reloaded.");

            return Result.Success(MapToResponseDto(updated), "Resource updated successfully.");
        }
        catch (DbUpdateException)
        {
            return Result.Failure<ResourceResponseDto>(400, "Unable to update resource. Please make sure tags are unique.");
        }
        catch (Exception ex)
        {
            return Result.Failure<ResourceResponseDto>(500, $"Failed to update resource: {ex.Message}");
        }
    }

    public async Task<Result> DeleteResourceAsync(int id, string userId)
    {
        try
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource is null)
                return Result.Failure(404, "Resource not found.");

            if (resource.AppUserId != userId)
                return Result.Failure(403, "You are not authorized to delete this resource.");

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return Result.Success("Resource deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failure(500, $"Failed to delete resource: {ex.Message}");
        }
    }

    private IQueryable<Resource> GetResourcesQuery()
    {
        return _context.Resources
            .Include(resourceItem => resourceItem.Author)
            .Include(resourceItem => resourceItem.Tags);
    }

    private static ResourceResponseDto MapToResponseDto(Resource resource)
    {
        return new ResourceResponseDto
        {
            Id = resource.Id,
            Title = resource.Title,
            Link = resource.Link,
            Description = resource.Description,
            CreatedAt = resource.CreatedAt,
            Tags = resource.Tags
                .OrderBy(tag => tag.Value)
                .Select(tag => tag.Value)
                .ToList(),
            Author = new ResourceAuthorDto
            {
                Id = resource.Author.Id,
                FirstName = resource.Author.FirstName,
                LastName = resource.Author.LastName
            }
        };
    }

    private static List<string> NormalizeTags(IEnumerable<string>? tags)
    {
        if (tags is null)
            return new List<string>();

        return tags
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim().ToLowerInvariant())
            .Distinct()
            .ToList();
    }
}
