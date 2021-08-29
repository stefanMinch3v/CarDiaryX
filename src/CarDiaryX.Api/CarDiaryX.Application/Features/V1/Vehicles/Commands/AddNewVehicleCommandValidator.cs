using CarDiaryX.Application.Common.Constants;
using FluentValidation;

namespace CarDiaryX.Application.Features.V1.Vehicles.Commands
{
    public class AddNewVehicleCommandValidator : AbstractValidator<AddNewVehicleCommand>
    {
        public AddNewVehicleCommandValidator()
        {
            this.RuleFor(u => u.RegistrationNumber)
                .NotEmpty()
                .WithMessage(ApplicationConstants.Vehicles.INVALID_PLATES_EMPTY);

            this.RuleFor(u => u.RegistrationNumber)
                .MaximumLength(ApplicationConstants.Vehicles.PLATES_MAX_SIZE)
                .WithMessage(ApplicationConstants.Vehicles.INVALID_PLATES_MAX_SIZE);
        }
    }
}
