using FluentValidation;
using CarDiaryX.Application.Common.Constants;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            this.RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage(ApplicationConstants.Users.INVALID_EMAIL_EMPTY);

            this.RuleFor(u => u.Email)
                .MinimumLength(ApplicationConstants.Users.EMAIL_MIN_LENGTH)
                .MaximumLength(ApplicationConstants.Users.EMAIL_MAX_LENGTH)
                .WithMessage(ApplicationConstants.Users.INVALID_EMAIL_LENGTH);

            this.RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage(ApplicationConstants.Users.INVALID_EMAIL_FORMAT);

            this.RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage(ApplicationConstants.Users.INVALID_PASSWORD_EMPTY);

            this.RuleFor(u => u.Password)
                .MinimumLength(ApplicationConstants.Users.PASSWORD_MIN_LENGTH)
                .MaximumLength(ApplicationConstants.Users.PASSWORD_MAX_LENGTH)
                .WithMessage(ApplicationConstants.Users.INVALID_PASSWORD_LENGTH);

            this.RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage(ApplicationConstants.Users.INVALID_FIRST_NAME_EMPTY);

            this.RuleFor(u => u.FirstName)
                .MinimumLength(ApplicationConstants.Users.NAME_MIN_LENGTH)
                .MaximumLength(ApplicationConstants.Users.NAME_MAX_LENGTH)
                .WithMessage(ApplicationConstants.Users.INVALID_FIRST_NAME_LENGTH);

            this.RuleFor(u => u.LastName)
                .NotEmpty()
                .WithMessage(ApplicationConstants.Users.INVALID_LAST_NAME_EMPTY);

            this.RuleFor(u => u.LastName)
                .MinimumLength(ApplicationConstants.Users.NAME_MIN_LENGTH)
                .MaximumLength(ApplicationConstants.Users.NAME_MAX_LENGTH)
                .WithMessage(ApplicationConstants.Users.INVALID_LAST_NAME_LENGTH);
        }
    }
}
