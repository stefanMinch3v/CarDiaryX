using FluentValidation;
using static CarDiaryX.Application.Common.Constants.ApplicationConstants;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            this.RuleFor(u => u.Email)
                .MinimumLength(Users.EMAIL_MIN_LENGTH)
                .MaximumLength(Users.EMAIL_MAX_LENGTH)
                .EmailAddress()
                .NotEmpty();

            this.RuleFor(u => u.Password)
                .MinimumLength(Users.PASSWORD_MIN_LENGTH)
                .MaximumLength(Users.PASSWORD_MAX_LENGTH)
                .NotEmpty();

            this.RuleFor(u => u.FirstName)
                .MinimumLength(Users.NAME_MIN_LENGTH)
                .MaximumLength(Users.NAME_MAX_LENGTH)
                .NotEmpty();

            this.RuleFor(u => u.LastName)
                .MinimumLength(Users.NAME_MIN_LENGTH)
                .MaximumLength(Users.NAME_MAX_LENGTH)
                .NotEmpty();

            this.RuleFor(u => u.Age)
                .GreaterThanOrEqualTo(Users.AGE_MIN)
                .LessThanOrEqualTo(Users.AGE_MAX);
        }
    }
}
