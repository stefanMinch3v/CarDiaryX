using CarDiaryX.Domain.Vehicles;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles
{
    public interface IPermissionRepository
    {
        Task AddDefault();
        Task<Permission> GetByUser(CancellationToken cancellationToken);
    }
}
