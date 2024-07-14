using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegistrationApp.Entities;
using ResistrationApp.Application.CountryService.DTOs;
using ResistrationApp.Application.UserService;
using ResistrationApp.Application.UserService.DTOs;
using System.ComponentModel.DataAnnotations;

namespace RegistrationApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO userDTO)
        {
            try
            {
                _logger.LogInformation("User registration attempt: {Email}, {ProvinceId}", userDTO.Email, userDTO.ProvinceId);

                await _userService.RegisterUser(userDTO);

                _logger.LogInformation("User registered successfully: {Email}, {ProvinceId}", userDTO.Email, userDTO.ProvinceId);
                return Ok();
            }
            catch (Exception ex) 
            {
                _logger.LogError("User registration failed: {Email}, {ProvinceId}, Error: {Error}", userDTO.Email, userDTO.ProvinceId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<CountryDTO>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve all users");

                var users = await _userService.GetUsers();

                _logger.LogInformation("Users retrieved successfully. Count = {count}", users.Count);

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving countries");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("isEmailEsists")]
        public async Task<ActionResult<bool>> IsEmailEsists(string email)
        {
            try
            {
                _logger.LogInformation("Checking email existing {email}", email);

                var result = await _userService.IsEmailExists(email);

                _logger.LogInformation("Email existing checked. Result = {result}", result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Checking email existing {email}", email);
                return BadRequest(ex.Message);
            }
        }
    }
}
