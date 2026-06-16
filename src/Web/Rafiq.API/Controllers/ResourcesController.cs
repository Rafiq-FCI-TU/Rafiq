using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.ResourceDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/resources")]
public class ResourcesController : BaseController
{
    private readonly IResourceService _resourceService;

    public ResourcesController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ResourceResponseDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateResource([FromBody] CreateResourceDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        var result = await _resourceService.CreateResourceAsync(userId, dto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return CreatedAtAction(
            nameof(GetResourceById),
            new { id = result.Value!.Id },
            new
            {
                success = true,
                message = result.Message,
                data = result.Value
            });
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ResourceResponseDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllResources()
        => (await _resourceService.GetAllResourcesAsync()).ToActionResult();

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ResourceResponseDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetResourceById(int id)
        => (await _resourceService.GetResourceByIdAsync(id)).ToActionResult();

    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ResourceResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateResource(int id, [FromBody] UpdateResourceDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _resourceService.UpdateResourceAsync(id, userId, dto)).ToActionResult();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteResource(int id)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _resourceService.DeleteResourceAsync(id, userId)).ToActionResult();
    }

    private string? GetUserId()
        => User.FindFirstValue("uid")
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
}
