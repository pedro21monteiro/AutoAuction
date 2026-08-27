using UserService.Models;

namespace UserService.Dao
{
    public interface IUserDao
    {
        Task<bool> ExistsAsync(string email, string username);
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
    }
}
