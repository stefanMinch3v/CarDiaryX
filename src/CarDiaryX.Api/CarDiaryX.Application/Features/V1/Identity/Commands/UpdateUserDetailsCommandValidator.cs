using FluentValidation;
using CarDiaryX.Application.Common.Constants;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class UpdateUserDetailsCommandValidator : AbstractValidator<UpdateUserDetailsCommand>
    {
        public UpdateUserDetailsCommandValidator()
        {
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
