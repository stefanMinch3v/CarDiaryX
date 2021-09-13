using CarDiaryX.Application.Common.BackgroundServices;
using CarDiaryX.Application.Features.V1.Vehicles.Commands;
using CarDiaryX.Application.Features.V1.Vehicles.OutputModels;
using CarDiaryX.Application.Features.V1.Vehicles.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarDiaryX.Web.Features.V1
{
    [Version(1)]
    [Authorize]
    public class VehiclesController : ApiControllerBase
    {
        private const string REGISTRATION_NUMBER_ROUTE = "{registrationNumber}";

        [HttpPost]
        public async Task<ActionResult> AddToUser([FromBody] AddNewVehicleCommand command)
            => await base.Send(command);

        [HttpDelete]
        [Route(REGISTRATION_NUMBER_ROUTE)]
        public async Task<ActionResult> RemoveFromUser(string registrationNumber)
            => await base.Send(new RemoveVehicleFromUserCommand { RegistrationNumber = registrationNumber });

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<RegistrationNumberOutputModel>>> GetAllRegistrationNumbers()
            => await base.Send(new GetAllRegistrationNumbersQuery());

        [HttpGet]
        [Route(REGISTRATION_NUMBER_ROUTE)]
        public async Task<ActionResult<VehicleSharedOutputModel>> GetInformation(string registrationNumber)
            => await base.Send(new GetVehicleInformationQuery { RegistrationNumber = registrationNumber });

        [HttpGet]
        [Route(REGISTRATION_NUMBER_ROUTE)]
        public async Task<ActionResult<VehicleSharedOutputModel>> GetDMR([FromQuery] GetVehicleDMRQuery query)
            => await base.Send(query);
    }
}
