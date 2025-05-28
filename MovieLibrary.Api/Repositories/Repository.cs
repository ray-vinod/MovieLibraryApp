using System.Linq.Expressions;

namespace MovieLibrary.Api.Repositories;

public class Repository<TEntity, TDataContext>(TDataContext context) : IRepository<TEntity>
    where TEntity : class
    where TDataContext : DbContext
{
    protected readonly TDataContext _context = context ?? throw new ArgumentNullException(nameof(context));
    internal readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);

    public IQueryable<TEntity> AsQueryable(bool includeAuditable = true) => includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();

    public async Task DeleteAsync(params object[] id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity not found");
        }

        _dbSet.Remove(entity);
    }

    public Task DeleteAsync(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeAuditable = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync(bool includeAuditable = true)
    {
        var query = includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeAuditable = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.Where(predicate).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(params object[] id) => await _dbSet.FindAsync(id);

    public async Task<PagedItem<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool includeAuditable = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Total count of items
        var totalItemCount = await query.CountAsync();

        // Items for the current page
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedItem<TEntity>
        {
            Items = items,
            TotalItemCount = (int)Math.Ceiling(totalItemCount / (double)pageSize),
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
    }

    public Task RestoreAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
        _dbSet.Attach(entity);

        return Task.CompletedTask;
    }

    public async Task SoftDeleteAsync(TEntity entity)
    {
        if (entity is IAuditable auditable)
        {
            auditable.IsDeleted = true;
            auditable.DeletedAt = DateTime.UtcNow;

            await UpdateAsync(entity);
        }
    }

    public Task UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _dbSet.Attach(entity);

        return Task.CompletedTask;
    }
}