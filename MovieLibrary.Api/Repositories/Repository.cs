using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.Repositories;

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
        var entity = await GetByIdAsync(id);
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

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeAuditable = true)
    {
        var query = includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();
        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync(bool includeAuditable = true)
    {
        var query = includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool includeAuditable = true)
    {
        var query = includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();
        return await query.Where(predicate).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(params object[] id) => await _dbSet.FindAsync(id);

    public async Task<PagedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool includeAuditable = true)
    {
        var query = includeAuditable ? _dbSet.AsQueryable() : _dbSet.AsQueryable().IgnoreQueryFilters();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var totalItems = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            TotalCount = (int)Math.Ceiling(totalItems / (double)pageSize),
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
    }

    public Task RestoreAsync(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        if (entity is IAuditable auditableEntity)
        {
            auditableEntity.IsDeleted = false;
            auditableEntity.DeletedAt = null;
        }

        _dbSet.Update(entity);

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
        else
        {
            await DeleteAsync(entity);
        }
    }

    public Task UpdateAsync(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _context.Entry(entity).State = EntityState.Modified;

        return Task.CompletedTask;
    }
}