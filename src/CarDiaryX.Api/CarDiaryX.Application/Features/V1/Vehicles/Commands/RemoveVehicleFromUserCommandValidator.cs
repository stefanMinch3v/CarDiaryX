using CarDiaryX.Application.Common.Constants;
using FluentValidation;

namespace CarDiaryX.Application.Features.V1.Vehicles.Commands
{
    public class RemoveVehicleFromUserCommandValidator : AbstractValidator<RemoveVehicleFromUserCommand>
    {
        public RemoveVehicleFromUserCommandValidator()
        {
            this.RuleFor(u => u.RegistrationNumber)
                .MaximumLength(ApplicationConstants.Vehicles.PLATES_MAX_SIZE)
                .NotEmpty();
        }
    }
}
