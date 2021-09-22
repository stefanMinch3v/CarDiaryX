using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Domain.VehicleServices;
using FluentValidation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.VehicleServices.Commands
{
    public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewCommandValidator()
        {
            this.RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_NAME_REVIEW_EMPTY);

            this.RuleFor(u => u.Name)
                .MinimumLength(ApplicationConstants.VehicleServices.NAME_MIN_LENGTH)
                .MaximumLength(ApplicationConstants.VehicleServices.NAME_REVIEW_MAX_LENGTH)
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_NAME_REVIEW);

            this.RuleFor(u => u.Prices)
                .MustAsync((u, t) => this.ValidatePricesEnum(u))
                .WithMessage(string.Format(ApplicationConstants.VehicleServices.INVALID_REVIEW_PRICES, this.GetValidPricesEnumList()));

            this.RuleFor(u => u.Ratings)
                .MustAsync((u, t) => this.ValidateRatingsEnum(u))
                .WithMessage(string.Format(ApplicationConstants.VehicleServices.INVALID_REVIEW_RATINGS, this.GetValidRatingsEnumList()));

            this.RuleFor(u => u.VehicleServiceId)
                .MustAsync((u, t) => this.ValidateVehicleServiceId(u))
                .WithMessage(ApplicationConstants.VehicleServices.INVALID_REVIEW_VEHICLE_SERVICE_TO_ATTACH);
        }

        private string GetValidPricesEnumList()
            => string.Join(",", Enum.GetValues(typeof(Prices)).Cast<int>());

        private string GetValidRatingsEnumList()
            => string.Join(",", Enum.GetValues(typeof(Ratings)).Cast<int>());

        private Task<bool> ValidatePricesEnum(int prices)
        {
            var isDefinedEnum = Enum.IsDefined(typeof(Prices), prices);

            if (!isDefinedEnum)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private Task<bool> ValidateRatingsEnum(int ratings)
        {
            var isDefinedEnum = Enum.IsDefined(typeof(Ratings), ratings);

            if (!isDefinedEnum)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private Task<bool> ValidateVehicleServiceId(int id)
        {
            if (id < 1)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
