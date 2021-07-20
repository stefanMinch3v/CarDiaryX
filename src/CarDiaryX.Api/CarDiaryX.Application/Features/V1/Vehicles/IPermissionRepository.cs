using CarDiaryX.Domain.Vehicles;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles
{
    public interface IPermissionRepository
    {
        Task AddDefault(string userId);
        Task<Permission> GetByUser(string userId);
    }
}
