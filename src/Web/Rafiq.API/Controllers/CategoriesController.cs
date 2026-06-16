using Microsoft.AspNetCore.Mvc;
using Rafiq.Application.Games;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllCategoriesAsync()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return HandleResult(result);
    }

    /// <summary>
    /// Retrieves a category with its games.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    [HttpGet("{id}/games")]
    public async Task<IActionResult> GetCategoryWithGamesAsync(int id)
    {
        var result = await _categoryService.GetCategoryWithGamesAsync(id);
        return HandleResult(result);
    }

    // get patient progress categories
    /// <summary>
    /// Retrieves categories relevant to patient progress.
    /// </summary>
    [HttpGet("patient-progress")]
    public async Task<IActionResult> GetPatientProgressCategoriesAsync(int patientId)
    {
        var result = await _categoryService.GetPatientProgressCategoriesAsync(patientId);
        return HandleResult(result);
    }

    private IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(),
            _ => StatusCode(result.StatusCode, result)
        };
    }
}
