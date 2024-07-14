using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegistrationApp.Core.Interfaces;
using RegistrationApp.Entities;
using ResistrationApp.Application.CountryService.DTOs;
using ResistrationApp.Application.PasswordService;
using ResistrationApp.Application.UserService.DTOs;

namespace ResistrationApp.Application.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UserService> _logger;
        private readonly IValidator<RegisterUserDTO> _validator;

        public UserService(
            IUserRepository userRepository, 
            IPasswordService passwordService, 
            ILogger<UserService> logger, 
            IValidator<RegisterUserDTO> validator)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
            _validator = validator;
        }

        public async Task<bool> IsEmailExists(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await _userRepository.GetUsers();

            var userDTOs = users.Select(x => new UserDTO 
            { 
                Id = x.Id,
                Email = x.Email, 
                PasswordHash = x.Password,
                ProvinceId = x.ProvinceId 
            }).ToList();

            return userDTOs;
        }

        public async Task RegisterUser(RegisterUserDTO userDTO)
        {
            await Validate(userDTO);
            User user = MapToUser(userDTO);

            await _userRepository.AddUser(user);
        }

        private User MapToUser(RegisterUserDTO userDTO)
        {
            // TODO: autoMapper
            var user = new User()
            {
                Email = userDTO.Email,
                Password = _passwordService.HashPassword(userDTO.Password),
                ProvinceId = userDTO.ProvinceId
            };

            _logger.LogDebug("UserDTO was mapped to User. User fields is: {Email}, {ProvinceId}, {PasswordHash}", userDTO.Email, userDTO.ProvinceId, userDTO.Password);
            return user;
        }

        private async Task Validate(RegisterUserDTO userDTO)
        {
            var validationResult = await _validator.ValidateAsync(userDTO);
            if (!validationResult.IsValid)
            {
                _logger.LogError("UserDTO validation failed: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
