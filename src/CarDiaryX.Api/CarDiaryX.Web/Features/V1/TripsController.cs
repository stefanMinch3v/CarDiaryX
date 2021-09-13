using CarDiaryX.Application.Features.V1.Trips.Commands;
using CarDiaryX.Application.Features.V1.Trips.OutputModels;
using CarDiaryX.Application.Features.V1.Trips.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarDiaryX.Web.Features.V1
{
    [Version(1)]
    [Authorize]
    public class TripsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AddNewTripCommand command)
            => await base.Send(command);

        [HttpGet]
        public async Task<ActionResult<TripWrapperOutputModel>> GetAll(int page = 1, string registrationNumber = null)
            => await base.Send(new GetAllTripsQuery { Page = page < 1 ? 1 : page, RegistrationNumber = registrationNumber });

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
            => await base.Send(new DeleteTripCommand { Id = id });

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateTripCommand command)
            => await base.Send(command);
    }
}
