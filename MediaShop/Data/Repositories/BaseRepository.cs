using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediaShop.Data.Repositories;

public class BaseRepository<T>(MediaShopContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly MediaShopContext _context = context;

    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(x=> x.Id == id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

}
