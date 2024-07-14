using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegistrationApp.Core.Interfaces;
using RegistrationApp.Infrastructure.Data;
using ResistrationApp.Application.PasswordService;
using ResistrationApp.Application.UserService.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private static RegistrationAppDbContext _registrationAppDbContext;
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices(services =>
        //    {
        //        var userRepositoryMock = new Mock<IUserRepository>();
        //        var passwordServiceMock = new Mock<IPasswordService>();

        //        // Set up mock behavior here if needed
        //        userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>())).Returns(Task.CompletedTask);
        //        passwordServiceMock.Setup(service => service.HashPassword(It.IsAny<string>())).Returns("hashedPassword123");

        //        services.AddScoped(_ => userRepositoryMock.Object);
        //        services.AddScoped(_ => passwordServiceMock.Object);
        //    });
        //}

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<RegistrationAppDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<RegistrationAppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<RegistrationAppDbContext>();

                    db.Database.EnsureCreated();
                }
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            return base.CreateHost(builder);
        }
    }
}
