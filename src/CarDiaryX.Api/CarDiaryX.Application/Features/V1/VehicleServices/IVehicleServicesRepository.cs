using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Features.V1.VehicleServices.InputModels;
using CarDiaryX.Domain.Common;
using CarDiaryX.Domain.VehicleServices;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.VehicleServices
{
    public interface IVehicleServicesRepository
    {
        Task<PagingModel<VehicleService>> GetAllShallow(
            CancellationToken cancellationToken,
            string searchText = null,
            int page = ApplicationConstants.PAGE,
            int pageSize = ApplicationConstants.PAGE_SIZE);

        Task<PagingModel<Review>> GetAllReviews(
            CancellationToken cancellationToken,
            int page = ApplicationConstants.PAGE,
            int pageSize = ApplicationConstants.PAGE_SIZE);

        Task<VehicleService> GetShallow(int id, CancellationToken cancellationToken);

        Task<bool> Add(string name, string description, AddressInputModel address);

        Task<bool> AddReview(string name, int vehicleServiceId, Ratings ratings, Prices prices);
    }
}
