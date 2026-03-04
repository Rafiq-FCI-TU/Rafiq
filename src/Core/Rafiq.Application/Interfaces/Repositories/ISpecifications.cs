using Microsoft.EntityFrameworkCore.Query;
using Rafiq.Domain.Common;
using System.Linq.Expressions;

namespace Rafiq.Application.Interfaces.Repositories;
public interface ISpecifications<T> where T : BaseEntity
{
    public Expression<Func<T, bool>> WhereCriteria { get; set; }
    List<IncludeSpec<T>> IncludesCriteria { get; }
    public Expression<Func<T, object>> OrderBy { get; set; }
    public Expression<Func<T, object>> OrderByDesc { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public bool IsPaginationEnabled { get; set; }
}

public class IncludeSpec<T>
{
    public Expression<Func<T, object>>? Include { get; set; }
    public string? IncludeString { get; set; }  // ✅ أضف هذا
    public List<string> ThenIncludes { get; set; } = new();
}