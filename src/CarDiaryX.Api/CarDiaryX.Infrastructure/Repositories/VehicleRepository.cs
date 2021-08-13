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

        public Task<(long TsId, long DataId, DateTimeOffset CreatedOn)> GetParamsForExternalCall(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleInformations
                .Where(vi => vi.RegistrationNumber == registrationNumber)
                .Select(vi => new Tuple<long, long, DateTimeOffset>(vi.DataTsId, vi.DataId, vi.CreatedOn).ToValueTuple())
                .FirstOrDefaultAsync(cancellationToken);

        public Task<VehicleDMR> GetDMR(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleDMRs.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);

        public Task<VehicleInformation> GetInformation(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleInformations.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);

        public Task<VehicleInspection> GetInspection(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.VehicleInspections.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);

        public async Task RemoveAllVehicleData(string userId, bool allRegistrationNumbers = true, string registrationNum = null)
        {
            if ((!allRegistrationNumbers && string.IsNullOrEmpty(registrationNum))
                || (allRegistrationNumbers && !string.IsNullOrEmpty(registrationNum)))
            {
                return;
            }

            UserRegistrationNumbers[] userRegNumbers = null;

            if (allRegistrationNumbers)
            {
                userRegNumbers = await this.dbContext.UserRegistrationNumbers
                    .Include(u => u.RegistrationNumber)
                    .Where(u => u.UserId == userId)
                    .ToArrayAsync();
            }
            else
            {
                userRegNumbers = await this.dbContext.UserRegistrationNumbers
                    .Include(u => u.RegistrationNumber)
                    .Where(u => u.UserId == userId && u.RegistrationNumber.Number == registrationNum)
                    .ToArrayAsync();
            }

            if (userRegNumbers is null || userRegNumbers.Length == 0)
            {
                return;
            }

            this.dbContext.UserRegistrationNumbers.RemoveRange(userRegNumbers);

            var numbersToDelete = new List<UserRegistrationNumbers>();

            foreach (var number in userRegNumbers)
            {
                var otherUsersHavingSameVehicle = (await this.dbContext.UserRegistrationNumbers
                    .Where(urn => urn.RegistrationNumberId == number.RegistrationNumberId)
                    .CountAsync()) > 1;

                if (!otherUsersHavingSameVehicle)
                {
                    numbersToDelete.Add(number);
                }
            }

            if (numbersToDelete.Count > 0)
            {
                foreach (var numberToDelete in numbersToDelete)
                {
                    var registrationNumber = numberToDelete.RegistrationNumber;

                    var vehicleInformation = await this.dbContext.VehicleInformations
                        .FirstOrDefaultAsync(vi => vi.RegistrationNumber == registrationNumber.Number);

                    if (vehicleInformation is not null)
                    {
                        this.dbContext.VehicleInformations.Remove(vehicleInformation);
                    }

                    var vehicleDmr = await this.dbContext.VehicleDMRs
                        .FirstOrDefaultAsync(dmr => dmr.RegistrationNumber == registrationNumber.Number);

                    if (vehicleDmr is not null)
                    {
                        this.dbContext.VehicleDMRs.Remove(vehicleDmr);
                    }

                    var vehicleInspection = await this.dbContext.VehicleInspections
                        .FirstOrDefaultAsync(i => i.RegistrationNumber == registrationNumber.Number);

                    if (vehicleInspection is not null)
                    {
                        this.dbContext.VehicleInspections.Remove(vehicleInspection);
                    }
                    
                    this.dbContext.RegistrationNumbers.Remove(registrationNumber);
                }
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task SaveDMR(string registrationNumber, DateTime? nextGreenTaxDate, DateTime? nextInspectionDate, string jsonData, string userId)
        {
            var dmr = new VehicleDMR
            {
                NextGreenTaxDate = nextGreenTaxDate,
                NextInspectionDate = nextInspectionDate,
                JsonData = jsonData,
                RegistrationNumber = registrationNumber,
                CreatedBy = userId
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

        public async Task SaveInformation(string registrationNumber, RootInformation information, string userId = null)
        {
            var informationDb = new VehicleInformation
            {
                DataId = information.Data.Id,
                DataTsId = information.Data.TsId,
                RegistrationNumber = registrationNumber,
                JsonData = information.RawData,
                CreatedBy = userId
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

        public async Task SaveInspection(string registrationNumber, string jsonData, string userId)
        {
            var inspection = new VehicleInspection
            {
                RegistrationNumber = registrationNumber,
                JsonData = jsonData,
                CreatedBy = userId
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

        public async Task UpdateInformation(string registrationNumber, RootInformation information, string userId)
        {
            var dbInformation = await this.dbContext.VehicleInformations.FirstOrDefaultAsync(i => i.RegistrationNumber == registrationNumber);

            if (dbInformation is null)
            {
                return;
            }

            dbInformation.JsonData = information.RawData;
            dbInformation.DataId = information.Data.Id;
            dbInformation.DataTsId = information.Data.TsId;
            dbInformation.ModifiedBy = userId;

            this.dbContext.VehicleInformations.Update(dbInformation);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateDMR(string registrationNumber, DateTime? nextGreenTaxDate, DateTime? nextInspectionDate, string jsonData, string userId)
        {
            var dbDMR = await this.dbContext.VehicleDMRs.FirstOrDefaultAsync(d => d.RegistrationNumber == registrationNumber);

            if (dbDMR is null)
            {
                return;
            }

            dbDMR.NextGreenTaxDate = nextGreenTaxDate;
            dbDMR.NextInspectionDate = nextInspectionDate;
            dbDMR.JsonData = jsonData;
            dbDMR.ModifiedBy = userId;

            this.dbContext.VehicleDMRs.Update(dbDMR);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateInspection(string registrationNumber, string jsonData, string userId)
        {
            var dbInspection = await this.dbContext.VehicleInspections.FirstOrDefaultAsync(i => i.RegistrationNumber == registrationNumber);

            if (dbInspection is null)
            {
                return;
            }

            dbInspection.JsonData = jsonData;
            dbInspection.ModifiedBy = userId;

            this.dbContext.VehicleInspections.Update(dbInspection);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
