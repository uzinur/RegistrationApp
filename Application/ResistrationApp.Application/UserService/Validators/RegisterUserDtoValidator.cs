using FluentValidation;
using FluentValidation.AspNetCore;
using RegistrationApp.Core.Interfaces;
using ResistrationApp.Application.UserService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistrationApp.Application.UserService.Validators
{
    public class RegisterUserDtoValidator: AbstractValidator<RegisterUserDTO>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserDtoValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MustAsync(BeUniqueEmail).WithMessage("The email address is already in use.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .Matches(@"\p{L}").WithMessage("Password must contain at least one letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number."); 

            RuleFor(x => x.ProvinceId).GreaterThan(0);

            RuleFor(x => x.IsAgree).Equal(true);
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _userRepository.EmailExistsAsync(email);
        }
    }
}
