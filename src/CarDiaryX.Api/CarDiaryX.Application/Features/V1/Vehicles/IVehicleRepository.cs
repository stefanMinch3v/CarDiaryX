using CarDiaryX.Domain.Integration;
using CarDiaryX.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles
{
    public interface IVehicleRepository
    {
        Task<VehicleInformation> GetInformation(string registrationNumber, CancellationToken cancellationToken);
        Task<VehicleDMR> GetDMR(string registrationNumber, CancellationToken cancellationToken);
        Task<VehicleInspection> GetInspection(string registrationNumber, CancellationToken cancellationToken);

        Task SaveInformation(string registrationNumber, RootInformation information);
        Task SaveDMR(string registrationNumber, DateTime? nextGreenTaxDate, DateTime? nextInspectionDate, string jsonData);
        Task SaveInspection(string registrationNumber, string jsonData);

        Task<IReadOnlyCollection<VehicleInformation>> GetInformationByUser(string userId, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<VehicleDMR>> GetDMRByUser(string userId, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<VehicleInspection>> GetInspectionsByUser(string userId, CancellationToken cancellationToken);
    }
}
