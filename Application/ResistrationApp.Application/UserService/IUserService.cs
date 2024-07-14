using Microsoft.AspNetCore.Mvc;
using ResistrationApp.Application.CountryService.DTOs;
using ResistrationApp.Application.UserService.DTOs;

namespace ResistrationApp.Application.UserService
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetUsers();
        Task<bool> IsEmailExists(string email);
        Task RegisterUser(RegisterUserDTO userDTO);
    }
}