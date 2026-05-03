using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Common;
using Rafiq.Application.Games;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Common;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Games;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Retrieves all game categories with summary details.
    /// </summary>
    public async Task<Result<IReadOnlyList<CategoryOverviewDto>>> GetAllCategoriesAsync()
    {
        try
        {
            var spec = new BaseSpecifications<Category>();
            spec.AddInclude("Games");

            var categoriesEntities = await _unitOfWork.Repository<Category>().GetAllAsync(spec);

            var categories = categoriesEntities.Select(category => new CategoryOverviewDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                GameCount = category.Games?.Count ?? 0
            }).ToList();

            return Result.Success<IReadOnlyList<CategoryOverviewDto>>(categories);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<CategoryOverviewDto>>(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a category with its related games.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    public async Task<Result<CategoryWithGamesDto>> GetCategoryWithGamesAsync(int categoryId)
    {
        try
        {
            var categorySpec = new BaseSpecifications<Category>
            {
                WhereCriteria = category => category.Id == categoryId
            };
            categorySpec.AddInclude("Games");

            var category = await _unitOfWork.Repository<Category>().GetEntityAsync(categorySpec);

            if (category is null)
            {
                return Result.Failure<CategoryWithGamesDto>(404, "Category not found.");
            }

            var result = new CategoryWithGamesDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Games = category.Games.Select(game => new GameSummaryDto
                {
                    Id = game.Id,
                    CategoryName = game.Category.Name,
                    Description = game.Description
                }).ToList()
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<CategoryWithGamesDto>(500, ex.Message);
        }
    }
}
