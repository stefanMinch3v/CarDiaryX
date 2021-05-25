using FluentValidation;
using static CarDiaryX.Application.Common.Constants.ApplicationConstants;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            this.RuleFor(u => u.NewPassword)
                .MinimumLength(Users.PASSWORD_MIN_LENGTH)
                .MaximumLength(Users.PASSWORD_MAX_LENGTH)
                .NotEmpty();
        }
    }
}
