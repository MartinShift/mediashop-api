using MediaShop.Data.Entities;

namespace MediaShop.Data.Interfaces;


public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);

    Task<List<Role>> GetAllAsync();

    Task<Role?> GetByNameAsync(string name);

    Task CreateAsync(Role role);

    void Update(Role role);

    Task DeleteAsync(int id);
}
