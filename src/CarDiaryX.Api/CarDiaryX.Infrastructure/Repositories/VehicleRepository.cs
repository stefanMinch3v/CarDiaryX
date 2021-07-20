using CarDiaryX.Application.Features.V1.Vehicles;
using CarDiaryX.Domain.Integration;
using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using CarDiaryX.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Repositories
{
    internal class VehicleRepository : IVehicleRepository
    {
        private readonly CarDiaryXDbContext dbContext;

        public VehicleRepository(CarDiaryXDbContext dbContext) 
            => this.dbContext = dbContext;

        public Task<VehicleDMR> GetDMR(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleDMRs.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);

        public async Task<IReadOnlyCollection<VehicleDMR>> GetDMRByUser(string userId, CancellationToken cancellationToken)
        {
            var registrationNumbers = await this.GetRegistrationNumbersByUser(userId, cancellationToken);

            return await this.dbContext.VehicleDMRs
                .Where(vi => registrationNumbers.Contains(vi.RegistrationNumber))
                .ToListAsync(cancellationToken);
        }

        public Task<VehicleInformation> GetInformation(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleInformations.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);

        public async Task<IReadOnlyCollection<VehicleInformation>> GetInformationByUser(string userId, CancellationToken cancellationToken)
        {
            var registrationNumbers = await this.GetRegistrationNumbersByUser(userId, cancellationToken);

            return await this.dbContext.VehicleInformations
                .Where(vi => registrationNumbers.Contains(vi.RegistrationNumber))
                .ToListAsync(cancellationToken);
        }

        public Task<VehicleInspection> GetInspection(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleInspections.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);

        public async Task<IReadOnlyCollection<VehicleInspection>> GetInspectionsByUser(string userId, CancellationToken cancellationToken)
        {
            var registrationNumbers = await this.GetRegistrationNumbersByUser(userId, cancellationToken);

            return await this.dbContext.VehicleInspections
                .Where(vi => registrationNumbers.Contains(vi.RegistrationNumber))
                .ToListAsync(cancellationToken);
        }

        // TODO: handle concurrent users as save information
        public async Task SaveDMR(string registrationNumber, DateTime? nextGreenTaxDate, DateTime? nextInspectionDate, string jsonData)
        {
            var dmr = new VehicleDMR
            {
                NextGreenTaxDate = nextGreenTaxDate,
                NextInspectionDate = nextInspectionDate,
                JsonData = jsonData,
                RegistrationNumber = registrationNumber
            };

            this.dbContext.VehicleDMRs.Add(dmr);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task SaveInformation(string registrationNumber, RootInformation information)
        {
            var informationDb = new VehicleInformation
            {
                DataId = information.Data.Id,
                DataTsId = information.Data.TsId,
                RegistrationNumber = registrationNumber,
                JsonData = information.RawData
            };

            try
            {
                this.dbContext.VehicleInformations.Add(informationDb);
                await this.dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                var isDuplicateKeyForPlates = e
                    ?.InnerException
                    ?.Message
                    .Contains(InfrastructureConstants.DUPLICATE_KEY_FOR_REGISTRATION_NUMBER_EXCEPTION_MESSAGE_IN_VI);

                if (!isDuplicateKeyForPlates.HasValue || !isDuplicateKeyForPlates.Value)
                {
                    throw;
                }

                this.dbContext.VehicleInformations.Local.Remove(informationDb);
            }
        }

        // TODO: handle concurrent users as save information
        public async Task SaveInspection(string registrationNumber, string jsonData)
        {
            var inspection = new VehicleInspection
            {
                RegistrationNumber = registrationNumber,
                JsonData = jsonData
            };

            this.dbContext.VehicleInspections.Add(inspection);
            await this.dbContext.SaveChangesAsync();
        }

        private Task<List<string>> GetRegistrationNumbersByUser(string userId, CancellationToken cancellationToken)
            => this.dbContext.RegistrationNumbers
                .Where(rn => rn.Users.Any(u => u.UserId == userId))
                .Select(rn => rn.Number)
                .ToListAsync(cancellationToken);
    }
}
