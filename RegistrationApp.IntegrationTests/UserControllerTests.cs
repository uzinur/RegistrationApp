using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RegistrationApp.Infrastructure.Data;
using RegistrationApp.WebAPI;
using ResistrationApp.Application.UserService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.IntegrationTests
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnOk_WhenUserIsValid()
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = "test@example.com",
                Password = "Password123",
                ProvinceId = 1,
                IsAgree = true,
            };
            var content = new StringContent(JsonConvert.SerializeObject(userDTO), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/users/register", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("", "Password123", 1, true)] // Empty Email
        [InlineData("invalidemail", "Password123", 1, true)] // Invalid Email
        [InlineData("test@example.com", "", 1, true)] // Empty Password
        [InlineData("test@example.com", "short", 1, true)] // Short Password
        [InlineData("test@example.com", "Password", 1, true)] // Password has no digits
        [InlineData("test@example.com", "1234567", 1, true)] // Password has no letters
        [InlineData("test@example.com", "Password123", 0, true)] // Invalid ProvinceId
        [InlineData("test@example.com", "Password123", 1, false)] // Not agreeing
        public async Task RegisterUser_ShouldReturnBadRequest_WhenUserIsInvalid(
         string email, string password, int provinceId, bool isAgree)
        {
            // Arrange
            var userDTO = new RegisterUserDTO
            {
                Email = email,
                Password = password,
                ProvinceId = provinceId,
                IsAgree = isAgree,
            };
            var content = new StringContent(JsonConvert.SerializeObject(userDTO), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/users/register", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
