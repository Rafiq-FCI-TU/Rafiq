using Rafiq.Domain.Common;
using System.Linq.Expressions;
using Rafiq.Application.Interfaces.Repositories;

namespace Rafiq.Application.Specifications;
public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
{
    public Expression<Func<T, bool>> WhereCriteria { get; set; } = null!;
    public List<IncludeSpec<T>> IncludesCriteria { get; set; } = [];
    public Expression<Func<T, object>> OrderBy { get; set; } = null!;
    public Expression<Func<T, object>> OrderByDesc { get; set; } = null!;
    public int Skip { get; set; }
    public int Take { get; set; }
    public bool IsPaginationEnabled { get; set; }

    public void AddInclude(string includeString)
    {
        IncludesCriteria.Add(new IncludeSpec<T>
        {
            IncludeString = includeString
        });
    }

    public void AddIncludeWithThenInclude(Expression<Func<T, object>> includeExpression, params string[] thenIncludes)
    {
        IncludesCriteria.Add(new IncludeSpec<T> 
        { 
            Include = includeExpression,
            ThenIncludes = thenIncludes.ToList()
        });
    }

    public void ApplyPagination(int skip, int take)
    {
        IsPaginationEnabled = true;
        Skip = skip;
        Take = take;
    }
}