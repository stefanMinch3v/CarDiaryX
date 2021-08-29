using FluentValidation;
using CarDiaryX.Application.Common.Constants;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            this.RuleFor(u => u.NewPassword)
                .NotEmpty()
                .WithMessage(ApplicationConstants.Users.INVALID_PASSWORD_EMPTY);

            this.RuleFor(u => u.NewPassword)
                .MinimumLength(ApplicationConstants.Users.PASSWORD_MIN_LENGTH)
                .MaximumLength(ApplicationConstants.Users.PASSWORD_MAX_LENGTH)
                .WithMessage(ApplicationConstants.Users.INVALID_PASSWORD_LENGTH);
        }
    }
}
