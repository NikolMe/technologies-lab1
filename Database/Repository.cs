using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Technologies.Models;

namespace Technologies.Database;

public class Repository<T> : IRepository<T>
    where T : BaseEntity
{
    private readonly DatabaseContext _context;

    public Repository(DatabaseContext context)
    {
        _context = context;
    }

    public Task<int> AddEntityAsync(T entity)
    {
        _context.Set<T>().Add(entity);

        return _context.SaveChangesAsync();
    }

    public Task AddRangeAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);

        return _context.SaveChangesAsync();
    }

    public Task<int> ExecuteUpdateAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setter)
    {
        return _context.Set<T>().Where(predicate).ExecuteUpdateAsync(setter);
    }

    public Task<T[]> GetArrayAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query.ToArrayAsync();
    }

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().AnyAsync(predicate);
    }

    public Task RemoveAsync(Expression<Func<T, bool>> predicate)
    {
        var set = _context.Set<T>().Where(predicate);

        _context.Set<T>().RemoveRange(set);

        return _context.SaveChangesAsync();
    }
}