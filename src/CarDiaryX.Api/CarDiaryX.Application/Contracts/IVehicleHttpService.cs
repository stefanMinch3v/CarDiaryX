using CarDiaryX.Domain.Integration;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Contracts
{
    public interface IVehicleHttpService
    {
        Task<RootInformation> GetInformation(string plates, CancellationToken cancellationToken);
        Task<RootDMR> GetDMR(long tsId, CancellationToken cancellationToken);
        Task<string> GetInspections(long id, CancellationToken cancellationToken);
    }
}
