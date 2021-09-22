using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Features.V1.Trips.InputModels;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Trips.Commands
{
    public class UpdateTripCommandValidator : AbstractValidator<UpdateTripCommand>
    {
        public UpdateTripCommandValidator()
        {
            this.RuleFor(u => u.Trip)
                .NotNull()
                .WithMessage(ApplicationConstants.Trips.INVALID_TRIP_NULL);

            this.RuleFor(u => u.Trip.RegistrationNumber)
                .NotEmpty()
                .WithMessage(ApplicationConstants.Vehicles.INVALID_PLATES_EMPTY);

            this.RuleFor(u => u.Trip.RegistrationNumber)
                .MaximumLength(ApplicationConstants.Vehicles.PLATES_MAX_SIZE)
                .WithMessage(ApplicationConstants.Vehicles.INVALID_PLATES_MAX_SIZE);

            this.RuleFor(u => u.Trip.Note)
                .MaximumLength(ApplicationConstants.Trips.NOTE_MAX_LENGTH)
                .WithMessage(ApplicationConstants.Trips.INVALID_NOTE_LENGTH);

            this.RuleFor(u => u.Trip.Distance)
                .LessThanOrEqualTo(ApplicationConstants.Trips.DISTANCE_MAX_LENGTH)
                .WithMessage(ApplicationConstants.Trips.INVALID_DISTANCE_LENGTH);

            this.RuleFor(u => u.Trip.Cost)
                .LessThanOrEqualTo(ApplicationConstants.Trips.COST_MAX_LENGTH)
                .WithMessage(ApplicationConstants.Trips.INVALID_COST_LENGTH);

            this.RuleFor(u => new { u.Trip.DepartureDate, u.Trip.ArrivalDate })
                .MustAsync((u, t) => this.DepartureDateMustBeBeforeArrivalDate(u.DepartureDate, u.ArrivalDate))
                .WithMessage(ApplicationConstants.Trips.INVALID_DEPARTURE_ARRIVAL_DATES);

            this.RuleFor(u => u.Trip.DepartureAddress)
                .NotNull()
                .WithMessage(ApplicationConstants.Trips.INVALID_DEPARTURE_ADDRESS_EMPTY);

            this.RuleFor(u => u.Trip.DepartureAddress)
                .MustAsync((u, t) => this.ValidateAddressName(u))
                .WithMessage(ApplicationConstants.Trips.INVALID_DEPARTURE_ADDRESS_NAME);

            this.RuleFor(u => u.Trip.DepartureAddress)
                .MustAsync((u, t) => this.ValidateAddressCoordinates(u))
                .WithMessage(ApplicationConstants.Trips.INVALID_DEPARTURE_ADDRESS_COORDINATES);

            this.RuleFor(u => u.Trip.ArrivalAddress)
                .NotNull()
                .WithMessage(ApplicationConstants.Trips.INVALID_ARRIVAL_ADDRESS_EMPTY);

            this.RuleFor(u => u.Trip.ArrivalAddress)
                .MustAsync((u, t) => this.ValidateAddressName(u))
                .WithMessage(ApplicationConstants.Trips.INVALID_ARRIVAL_ADDRESS_NAME);

            this.RuleFor(u => u.Trip.ArrivalAddress)
                .MustAsync((u, t) => this.ValidateAddressCoordinates(u))
                .WithMessage(ApplicationConstants.Trips.INVALID_ARRIVAL_ADDRESS_COORDINATES);
        }

        private Task<bool> DepartureDateMustBeBeforeArrivalDate(DateTimeOffset departure, DateTimeOffset arrival)
        {
            if (departure >= arrival)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private Task<bool> ValidateAddressName(AddressInputModel address)
            => Task.FromResult(AddressHelper.HasValidName(address));

        private Task<bool> ValidateAddressCoordinates(AddressInputModel address)
            => Task.FromResult(AddressHelper.HasValidCoordinates(address));
    }
}
