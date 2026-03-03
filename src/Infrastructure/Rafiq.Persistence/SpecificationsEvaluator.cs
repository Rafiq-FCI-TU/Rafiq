using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Domain.Common;
using System.Linq.Expressions;

namespace Rafiq.Persistence;

public class SpecificationsEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> spec)
    {
        var query = inputQuery;

        // -------- Where --------
        if (spec.WhereCriteria != null)
            query = query.Where(spec.WhereCriteria);

        // -------- Order --------
        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDesc != null)
            query = query.OrderByDescending(spec.OrderByDesc);

        // -------- Pagination --------
        if (spec.IsPaginationEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);

        // -------- Includes --------
        foreach (var includeSpec in spec.IncludesCriteria)
        {
            // Case 1: Include by full string
            if (!string.IsNullOrWhiteSpace(includeSpec.IncludeString))
            {
                query = query.Include(includeSpec.IncludeString);
                continue;
            }

            // Case 2: Include + ThenIncludes
            if (includeSpec.Include != null)
            {
                // Include الأساسي
                query = query.Include(includeSpec.Include);

                if (includeSpec.ThenIncludes != null && includeSpec.ThenIncludes.Any())
                {
                    var root = GetPropertyName(includeSpec.Include);

                    foreach (var thenInclude in includeSpec.ThenIncludes)
                    {
                        var fullPath = $"{root}.{thenInclude}";
                        query = query.Include(fullPath);
                    }
                }
            }
        }

        return query;
    }

    // Helper: a => a.Categories  -->  "Categories"
    private static string GetPropertyName(Expression<Func<T, object>> expression)
    {
        if (expression.Body is MemberExpression member)
            return member.Member.Name;

        if (expression.Body is UnaryExpression unary &&
            unary.Operand is MemberExpression unaryMember)
            return unaryMember.Member.Name;

        throw new InvalidOperationException("Invalid include expression");
    }
}