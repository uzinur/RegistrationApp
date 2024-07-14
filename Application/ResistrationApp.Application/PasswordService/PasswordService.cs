using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistrationApp.Application.PasswordService
{
    public class PasswordService : IPasswordService
    {
        // Метод для хеширования пароля
        public string HashPassword(string password)
        {
            // Генерация соли и хеширование пароля
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Метод для проверки пароля
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Проверка пароля с использованием хеша
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
