using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Common;
using Rafiq.Domain.Common;
using Rafiq.Persistence.Data;

namespace Rafiq.Infrastructure.Common;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly RafiqDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(RafiqDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);

    public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AsNoTracking().AnyAsync(predicate);

    public IQueryable<T> Query() => _dbSet;

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);
}
