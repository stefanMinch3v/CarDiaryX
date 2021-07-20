using FluentValidation;
using static CarDiaryX.Application.Common.Constants.ApplicationConstants;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class UpdateUserDetailsCommandValidator : AbstractValidator<UpdateUserDetailsCommand>
    {
        public UpdateUserDetailsCommandValidator()
        {
            this.RuleFor(u => u.FirstName)
                .MinimumLength(Users.NAME_MIN_LENGTH)
                .MaximumLength(Users.NAME_MAX_LENGTH)
                .NotEmpty();

            this.RuleFor(u => u.LastName)
                .MinimumLength(Users.NAME_MIN_LENGTH)
                .MaximumLength(Users.NAME_MAX_LENGTH)
                .NotEmpty();
        }
    }
}
