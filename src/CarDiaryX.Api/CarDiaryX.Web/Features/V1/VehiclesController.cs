using CarDiaryX.Application.Features.V1.Vehicles.Commands;
using CarDiaryX.Application.Features.V1.Vehicles.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        public async Task<ActionResult<object>> GetAllRegistrationNumbers()
            => await base.Send(new GetAllRegistrationNumbersQuery());

        [HttpGet]
        public async Task<ActionResult<object>> Get(string registrationNumber)
            => await base.Send(new GetVehicleQuery { RegistrationNumber = registrationNumber });
    }
}
