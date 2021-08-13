using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Domain.Integration;
using CarDiaryX.Domain.Vehicles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles
{
    public interface IVehicleRepository
    {
        Task<VehicleInformation> GetInformation(string registrationNumber, CancellationToken cancellationToken);
        Task<VehicleDMR> GetDMR(string registrationNumber, CancellationToken cancellationToken);
        Task<VehicleInspection> GetInspection(string registrationNumber, CancellationToken cancellationToken);
        Task<(long TsId, long DataId, DateTimeOffset CreatedOn)> GetParamsForExternalCall(string registrationNumber, CancellationToken cancellationToken);

        Task SaveInformation(string registrationNumber, RootInformation information, string userId = null);
        Task SaveDMR(string registrationNumber, DateTime? nextGreenTaxDate, DateTime? nextInspectionDate, string jsonData, string userId);
        [Obsolete(ApplicationConstants.HALTED_FEATURES)]
        Task SaveInspection(string registrationNumber, string jsonData, string userId);

        Task UpdateInformation(string registrationNumber, RootInformation information, string userId);
        Task UpdateDMR(string registrationNumber, DateTime? nextGreenTaxDate, DateTime? nextInspectionDate, string jsonData, string userId);
        [Obsolete(ApplicationConstants.HALTED_FEATURES)]
        Task UpdateInspection(string registrationNumber, string jsonData, string userId);

        Task RemoveAllVehicleData(string userId, bool allRegistrationNumbers = true, string registrationNumber = null);
    }
}
