using MediaShop.Data.Entities;

namespace MediaShop.Data.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T entity);

    T Update(T entity);

    void Delete(T entity);

    Task<T?> GetByIdAsync(int id);

    Task<IEnumerable<T>> GetAllAsync();
}
