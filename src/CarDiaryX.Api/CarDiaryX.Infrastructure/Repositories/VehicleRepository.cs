using CarDiaryX.Application.Features.V1.Vehicles;
using CarDiaryX.Domain.Integration;
using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using CarDiaryX.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
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

        public Task<VehicleInformation> GetInformation(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleInformations.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);

        public Task<VehicleInspection> GetInspection(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleInspections.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);
        
        public async Task SaveDMR(string registrationNumber, DateTime? nextGreenTaxDate, DateTime? nextInspectionDate, string jsonData)
        {
            var dmr = new VehicleDMR
            {
                NextGreenTaxDate = nextGreenTaxDate,
                NextInspectionDate = nextInspectionDate,
                JsonData = jsonData,
                RegistrationNumber = registrationNumber
            };
            
            try
            {
                this.dbContext.VehicleDMRs.Add(dmr);
                await this.dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                var isDuplicateKeyForPlates = e
                    .InnerException
                    ?.Message
                    .Contains(InfrastructureConstants.DUPLICATE_KEY_FOR_REGISTRATION_NUMBER_EXCEPTION_MESSAGE_IN_DMR);

                if (!isDuplicateKeyForPlates.HasValue || !isDuplicateKeyForPlates.Value)
                {
                    throw;
                }

                this.dbContext.VehicleDMRs.Local.Remove(dmr);
            }
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
                    .InnerException
                    ?.Message
                    .Contains(InfrastructureConstants.DUPLICATE_KEY_FOR_REGISTRATION_NUMBER_EXCEPTION_MESSAGE_IN_VI);

                if (!isDuplicateKeyForPlates.HasValue || !isDuplicateKeyForPlates.Value)
                {
                    throw;
                }

                this.dbContext.VehicleInformations.Local.Remove(informationDb);
            }
        }
        
        public async Task SaveInspection(string registrationNumber, string jsonData)
        {
            var inspection = new VehicleInspection
            {
                RegistrationNumber = registrationNumber,
                JsonData = jsonData
            };

            try
            {
                this.dbContext.VehicleInspections.Add(inspection);
                await this.dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                var isDuplicateKeyForPlates = e
                    .InnerException
                    ?.Message
                    .Contains(InfrastructureConstants.DUPLICATE_KEY_FOR_REGISTRATION_NUMBER_EXCEPTION_MESSAGE_IN_VII);

                if (!isDuplicateKeyForPlates.HasValue || !isDuplicateKeyForPlates.Value)
                {
                    throw;
                }

                this.dbContext.VehicleInspections.Local.Remove(inspection);
            }
        }
    }
}
