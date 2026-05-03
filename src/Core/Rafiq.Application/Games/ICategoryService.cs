using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Games;

public interface ICategoryService
{
    /// <summary>
    /// Retrieves all game categories with summary details.
    /// </summary>
    Task<Result<IReadOnlyList<CategoryOverviewDto>>> GetAllCategoriesAsync();

    /// <summary>
    /// Retrieves a category with its related games.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    Task<Result<CategoryWithGamesDto>> GetCategoryWithGamesAsync(int categoryId);
}
