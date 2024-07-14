using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RegistrationApp.Core.Interfaces;
using RegistrationApp.Infrastructure.Data;
using ResistrationApp.Application.CountryService;
using ResistrationApp.Application.PasswordService;
using ResistrationApp.Application.UserService;
using ResistrationApp.Application.UserService.Validators;
using Serilog;

namespace RegistrationApp.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            RegisterApplicationServices(builder);

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            // Configure Fluent Validations
            //builder.Services.AddFluentValidationAutoValidation(); //Switched off, decided to use application layer validation with unit testing.
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddDbContext<RegistrationAppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Log.Information("Application starting up");

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.InitializeDatabase<RegistrationAppDbContext>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        static void RegisterApplicationServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
        }
    }
}
