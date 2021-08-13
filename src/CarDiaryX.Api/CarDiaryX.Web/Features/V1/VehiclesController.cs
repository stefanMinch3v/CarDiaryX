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

        //[HttpGet("{typeId:int}")]
        //public async Task<ActionResult<IReadOnlyCollection<VehicleSharedOutputModel>>> Brands(int typeId)
        //    => await base.Send(new GetVehicleBrandsQuery(typeId));

        [HttpPost]
        public async Task<ActionResult> AddToUser(AddNewVehicleCommand command)
            => await base.Send(command);

        [HttpDelete]
        public async Task<ActionResult> RemoveFromUser([FromQuery] RemoveVehicleFromUserCommand command)
            => await base.Send(command);

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<RegistrationNumberOutputModel>>> GetAllRegistrationNumbers()
            => await base.Send(new GetAllRegistrationNumbersQuery());

        [HttpGet]
        public async Task<ActionResult<VehicleSharedOutputModel>> GetInformation(string registrationNumber)
            => await base.Send(new GetVehicleInformationQuery { RegistrationNumber = registrationNumber });

        [HttpGet]
        public async Task<ActionResult<VehicleSharedOutputModel>> GetDMR(string registrationNumber)
            => await base.Send(new GetVehicleDMRQuery { RegistrationNumber = registrationNumber });
    }
}
