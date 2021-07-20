using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Domain.Integration;
using CarDiaryX.Domain.Vehicles;
using MediatR;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Commands
{
    public class AddNewVehicleCommand : IRequest<Result>
    {
        public string RegistrationNumber { get; set; }

        public class CreateVehicleCommandHandler : IRequestHandler<AddNewVehicleCommand, Result>
        {
            private readonly IVehicleHttpService vehicleHttpService;
            private readonly IVehicleRepository vehicleRepository;
            private readonly IRegistrationNumberRepository registrationNumberRepository;
            private readonly IPermissionRepository permissionRepository;
            private readonly ICurrentUser currentUser;

            public CreateVehicleCommandHandler(
                IVehicleHttpService vehicleHttpService, 
                IVehicleRepository vehicleRepository,
                IRegistrationNumberRepository registrationNumberRepository,
                IPermissionRepository permissionRepository,
                ICurrentUser currentUser)
            {
                this.vehicleHttpService = vehicleHttpService;
                this.vehicleRepository = vehicleRepository;
                this.registrationNumberRepository = registrationNumberRepository;
                this.permissionRepository = permissionRepository;
                this.currentUser = currentUser;
            }

            public async Task<Result> Handle(AddNewVehicleCommand request, CancellationToken cancellationToken)
            {
                // TODO: permissions table count external calls
                var existingRegistrationNumber = await this.registrationNumberRepository.Get(request.RegistrationNumber);
                if (existingRegistrationNumber is not null)
                {
                    await this.registrationNumberRepository.AddToUser(existingRegistrationNumber.Id, this.currentUser.UserId);
                    return Result.Success;
                }

                var vehicleRootInfo = await this.vehicleHttpService.GetInformation(request.RegistrationNumber, cancellationToken);
                if (vehicleRootInfo is null)
                {
                    var errors = new[] { ApplicationConstants.External.SERVER_IS_NOT_RESPONDING };
                    return Result.Failure(errors);
                }

                await this.vehicleRepository.SaveInformation(request.RegistrationNumber, vehicleRootInfo);

                var permission = await this.permissionRepository.GetByUser(this.currentUser.UserId);
                if (permission?.PermissionType == PermissionType.Premium 
                    || permission?.PermissionType == PermissionType.Professional)
                {
                    // TODO
                    var vehicleDMRRawContent = await this.vehicleHttpService.GetDMR(
                        vehicleRootInfo.Data.TsId, 
                        cancellationToken); // slow request 5-8 seconds

                    if (vehicleDMRRawContent is null)
                    {
                        var errors = new[] { ApplicationConstants.External.SERVER_IS_NOT_RESPONDING };
                        return Result.Failure(errors);
                    }

                    // TODO: parse data: nextGreenTaxDate, nextInspectionDate
                    await this.vehicleRepository.SaveDMR(
                        request.RegistrationNumber, 
                        DateTime.Now, //nextGreenTaxDate
                        DateTime.Now, //nextInspectionDate
                        vehicleDMRRawContent);

                    //var vehicleInspections = await this.vehicleHttpService.GetInspections(vehicleInfo.Data.Id, cancellationToken); //4518851
                }

                var shortDescription = this.BuildShortDescription(vehicleRootInfo);
                var savedId = await this.registrationNumberRepository.Save(request.RegistrationNumber, shortDescription);
                await this.registrationNumberRepository.AddToUser(savedId, this.currentUser.UserId);

                return Result.Success;
                //return new
                //{
                //    info = vehicleRootInfo,
                //    inspections = vehicleInspections,
                //    dmr = vehicleDMR,
                //    jdmr = JsonConvert.DeserializeObject<JObject>(vehicleDMR),
                //    jInpsections = JsonConvert.DeserializeObject<JArray>(vehicleInspections),
                //};
            }

            private string BuildShortDescription(RootInformation rootInformation)
            {
                if (rootInformation is null)
                {
                    return null;
                }

                const string identifier = "^";
                var sb = new StringBuilder();

                sb.Append(rootInformation.Data.Brand);
                sb.Append(identifier);
                sb.Append(rootInformation.Data.Model);
                sb.Append(identifier);
                sb.Append(rootInformation.Data.Version ??= string.Empty);
                sb.Append(identifier);
                sb.Append(rootInformation.Data.ModelYear?.ToString());
                sb.Append(identifier);
                sb.Append(rootInformation.Data.FuelType ??= string.Empty);

                return sb.ToString().TrimEnd();
            }
        }
    }
}
