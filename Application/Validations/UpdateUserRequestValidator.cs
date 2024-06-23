using Application.Models;
using Application.Utilities;
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
                .Must(Extensions.HasUpperCase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(Extensions.HasLowerCase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(Extensions.HasNumeric).WithMessage("Password must contain at least one numeric character.")
            .Must(Extensions.HasSpecialCharacter).WithMessage("Password must contain at least one special character.")
            .Must(Extensions.IsValidLength).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Enter a valid value")
                .Must(Extensions.HasUpperCase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(Extensions.HasLowerCase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(Extensions.HasNumeric).WithMessage("Password must contain at least one numeric character.")
            .Must(Extensions.HasSpecialCharacter).WithMessage("Password must contain at least one special character.")
            .Must(Extensions.IsValidLength).WithMessage("Password must be at least 8 characters long.");
        }
    }
}
