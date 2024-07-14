using RegistrationApp.Entities;

namespace RegistrationApp.Core.Interfaces
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<bool> EmailExistsAsync(string email);
        Task<List<User>> GetUsers();
    }
}
