using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RegistrationApp.Core.Interfaces;
using RegistrationApp.Entities;
using ResistrationApp.Application.CountryService;
using ResistrationApp.Application.PasswordService;
using ResistrationApp.Application.UserService;
using ResistrationApp.Application.UserService.DTOs;
using ResistrationApp.Application.UserService.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationApp.Application.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _usersRepositoryMock;
        private Mock<ILogger<UserService>> _loggerMock;
        private Mock<IPasswordService> _passwordServiceMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _usersRepositoryMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_usersRepositoryMock.Object, _passwordServiceMock.Object, _loggerMock.Object, new RegisterUserDtoValidator(_usersRepositoryMock.Object));
        }

        [Test]
        public async Task GetUsers_ReturnsListOfUserDTOs()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Email = "emeail1@company,com", Password = "someHash1", ProvinceId = 1 },
                new User { Id = 2, Email = "emeail2@company,com", Password = "someHash1", ProvinceId = 1 },
            };

            _usersRepositoryMock.Setup(repo => repo.GetUsers()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsers();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("UserDTO", result[0].GetType().Name);
            Assert.AreEqual("emeail1@company,com", result[0].Email);
            Assert.AreEqual("emeail2@company,com", result[1].Email);
        }

        [Test]
        public async Task RegisterUser_HashPassword_AndCallsAddUser()
        {
            // Arrange
            var userDTO = new RegisterUserDTO 
            { 
                Email = "test@example.com", 
                Password = "password123", 
                ProvinceId = 1,
                IsAgree = true
            };

            var hashedPassword = "hashedPassword123";
            _passwordServiceMock.Setup(x => x.HashPassword(userDTO.Password)).Returns(hashedPassword);

            // Act
            await _userService.RegisterUser(userDTO);

            // Assert
            // AddUser was called
            _usersRepositoryMock.Verify(repo => repo.AddUser(It.Is<User>(u =>
                u.Email == userDTO.Email &&
                u.Password == hashedPassword &&
                u.ProvinceId == userDTO.ProvinceId
            )), Times.Once);

            //Password hashing was called
            _passwordServiceMock.Verify(x => x.HashPassword(It.Is<string>(p => p == userDTO.Password)), Times.Once);
        }

        [Test]
        public async Task RegisterUser_LogsInformation()
        {
            // Arrange
            var userDTO = new RegisterUserDTO()
            {
                Email = "test@example.com",
                Password = "password123",
                ProvinceId = 1,
                IsAgree = true
            };
            _usersRepositoryMock.Setup(x => x.AddUser(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            await _userService.RegisterUser(userDTO);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("UserDTO was mapped to User")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Test]
        [TestCase("", "password123", 1, true, "Email")]
        [TestCase("invalid-email", "password123", 1, true, "Email")]
        [TestCase("test@example.com", "123456", 1, true, "Password")]
        [TestCase("test@example.com", "password", 1, true, "Password")]
        [TestCase("test@example.com", "password123", 0, true, "ProvinceId")]
        [TestCase("test@example.com", "password123", 1, false, "IsAgree")]
        [TestCase("existing@example.com", "password123", 1, false, "Email")]
        public void RegisterUser_InvalidValidation(string email, string password, int provinceId, bool isAgree, string expectedErrorField)
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = email,
                Password = password,
                ProvinceId = provinceId,
                IsAgree = isAgree,
            };
            if (email == "existing@example.com")
            {
                _usersRepositoryMock.Setup(x => x.EmailExistsAsync(email)).ReturnsAsync(true);
            }

            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await _userService.RegisterUser(userDTO));

            // Assert
            Assert.IsTrue(ex.Errors.Any(e => e.PropertyName == expectedErrorField));
        }
    }
}
