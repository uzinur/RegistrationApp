using Microsoft.EntityFrameworkCore;
using RegistrationApp.Core.Interfaces;
using RegistrationApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly RegistrationAppDbContext _registrationAppDbContext;

        public UserRepository(RegistrationAppDbContext registrationAppDbContext)
        {
            _registrationAppDbContext = registrationAppDbContext;
        }

        public async Task AddUser(User user)
        {
            _registrationAppDbContext.Users.Add(user);

            await _registrationAppDbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await _registrationAppDbContext.Users.ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _registrationAppDbContext.Users.AnyAsync(u => u.Email == email);
        }
    }
}
