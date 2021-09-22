using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Features.V1.VehicleServices.InputModels;
using FluentValidation;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.VehicleServices.Commands
{
    public class AddVehicleServiceCommandValidator : AbstractValidator<AddVehicleServiceCommand>
    {
        public AddVehicleServiceCommandValidator()
        {
            this.RuleFor(u => u.Address)
                .NotNull()
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_ADDRESS_EMPTY);

            this.RuleFor(u => u.Address)
                .MustAsync((u, t) => this.ValidateAddressName(u))
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_ADDRESS);

            this.RuleFor(u => u.Address)
                .MustAsync((u, t) => this.ValidateAddressCoordinates(u))
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_ADDRESS_COORDINATES);

            this.RuleFor(u => u.Description)
                .NotEmpty()
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_DESCRIPTION_EMPTY);

            this.RuleFor(u => u.Description)
                .MinimumLength(ApplicationConstants.VehicleServices.DESCRIPTION_MIN_LENGTH)
                .MaximumLength(ApplicationConstants.VehicleServices.DESCRIPTION_MAX_LENGTH)
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_DESCRIPTION);

            this.RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_NAME_EMPTY);

            this.RuleFor(u => u.Name)
                .MinimumLength(ApplicationConstants.VehicleServices.NAME_MIN_LENGTH)
                .MaximumLength(ApplicationConstants.VehicleServices.NAME_MAX_LENGTH)
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_NAME);
        }

        private Task<bool> ValidateAddressName(AddressInputModel address)
            => Task.FromResult(AddressHelper.HasValidName(address));

        private Task<bool> ValidateAddressCoordinates(AddressInputModel address)
            => Task.FromResult(AddressHelper.HasValidCoordinates(address));
    }
}
