using MediaShop.Data.Entities;

namespace MediaShop.Data.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);

    Task<List<User>> GetAllAsync();

    Task<User?> GetByUsernameAsync(string username);

    Task CreateAsync(User user);

    void Update(User user);

    Task DeleteAsync(int id);

    Task<List<Role>> GetRolesAsync(int id);

    Task<List<User>> GetUsersByRoleAsync(string roleName);

    Task<User?> GetByEmailAsync(string email);

    Task<double> GetAverageRatingAsync(int id);

    Task<User?> GetNoTrackingAsync(int id);
}

