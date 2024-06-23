using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.ReferenceId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Enter a valid value").EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter a valid value")
                .Must(HasUpperCase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(HasLowerCase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(HasNumeric).WithMessage("Password must contain at least one numeric character.")
            .Must(HasSpecialCharacter).WithMessage("Password must contain at least one special character.")
            .Must(IsValidLength).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Enter a valid value")
                .Must(HasUpperCase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(HasLowerCase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(HasNumeric).WithMessage("Password must contain at least one numeric character.")
            .Must(HasSpecialCharacter).WithMessage("Password must contain at least one special character.")
            .Must(IsValidLength).WithMessage("Password must be at least 8 characters long.");
        }

        private bool HasUpperCase(string value)
        {
            return value.Any(char.IsUpper);
        }

        private bool HasLowerCase(string value)
        {
            return value.Any(char.IsLower);
        }

        private bool HasNumeric(string value)
        {
            return value.Any(char.IsDigit);
        }

        private bool HasSpecialCharacter(string value)
        {
            var specialCharacters = "!@#$%^&*(),.?\":{}|<>";
            return value.Any(c => specialCharacters.Contains(c));
        }
        private bool IsValidLength(string value)
        {
            return value.Length >= 8;
        }
    }
}
