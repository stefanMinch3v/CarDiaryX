using CarDiaryX.Domain.Integration;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles
{
    public interface IVehicleHttpService
    {
        Task<RootInformation> GetInformation(string plates, CancellationToken cancellationToken);
        Task<string> GetDMR(long tsId, CancellationToken cancellationToken);
        Task<string> GetInspections(long id, CancellationToken cancellationToken);
    }
}
