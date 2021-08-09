using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Vehicles;
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
    internal class RegistrationNumberRepository : IRegistrationNumberRepository
    {
        private readonly CarDiaryXDbContext dbContext;
        private readonly ICurrentUser currentUser;

        public RegistrationNumberRepository(CarDiaryXDbContext dbContext, ICurrentUser currentUser)
        {
            this.dbContext = dbContext;
            this.currentUser = currentUser;
        }

        public async Task AddToUser(Guid registrationNumberId)
        {
            var registrationNumber = await this.dbContext.RegistrationNumbers.FindAsync(registrationNumberId);

            if (registrationNumber is null)
            {
                return;
            }

            var existingRelation = await this.dbContext.UserRegistrationNumbers
                .AsNoTracking()
                .AnyAsync(ur => ur.RegistrationNumberId == registrationNumberId
                    && ur.UserId == this.currentUser.UserId);

            if (existingRelation)
            {
                return;
            }

            var userRegistrationNumber = new UserRegistrationNumbers
            {
                RegistrationNumberId = registrationNumberId,
                UserId = this.currentUser.UserId
            };

            registrationNumber.Users.Add(userRegistrationNumber);

            this.dbContext.RegistrationNumbers.Update(registrationNumber);
            await this.dbContext.SaveChangesAsync();
        }

        public Task<RegistrationNumber> Get(string registrationNumber, CancellationToken cancellationToken)
            => this.dbContext.RegistrationNumbers.FirstOrDefaultAsync(rn => rn.Number == registrationNumber, cancellationToken);

        public async Task<IReadOnlyCollection<RegistrationNumber>> GetByUser(CancellationToken cancellationToken)
            => await this.dbContext.RegistrationNumbers
                .Where(rn => rn.Users.Any(u => u.UserId == this.currentUser.UserId))
                .OrderBy(rn => rn.CreatedOn)
                .ToArrayAsync(cancellationToken);

        public async Task<Guid> Save(string registrationNumber, string shortDescription)
        {
            var number = new RegistrationNumber
            {
                Number = registrationNumber,
                ShortDescription = shortDescription
            };

            try
            {
                this.dbContext.RegistrationNumbers.Add(number);
                await this.dbContext.SaveChangesAsync();

                return number.Id;
            }
            catch (DbUpdateException e)
            {
                var isDuplicateKeyForPlates = e
                    ?.InnerException
                    ?.Message
                    .Contains(InfrastructureConstants.DUPLICATE_KEY_FOR_REGISTRATION_NUMBER_EXCEPTION_MESSAGE_IN_RN);

                if (!isDuplicateKeyForPlates.HasValue || !isDuplicateKeyForPlates.Value)
                {
                    throw;
                }

                this.dbContext.RegistrationNumbers.Local.Remove(number);

                var existingNumber = await this.dbContext.RegistrationNumbers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(rn => rn.Number == registrationNumber);

                return existingNumber?.Id ?? throw new DbUpdateException(
                    InfrastructureConstants.SAVED_REGISTRATION_NUMBER_EXCEPTION_IN_DUPLICATE_AND_UNEXISTING_STATE,
                    e);
            }
        }
    }
}
