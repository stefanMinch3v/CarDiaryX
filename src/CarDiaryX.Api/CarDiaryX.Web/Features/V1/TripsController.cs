using CarDiaryX.Application.Features.V1.Trips.Commands;
using CarDiaryX.Application.Features.V1.Trips.OutputModels;
using CarDiaryX.Application.Features.V1.Trips.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarDiaryX.Web.Features.V1
{
    [Version(1)]
    [Authorize]
    public class TripsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Add(AddNewTripCommand command)
            => await base.Send(command);

        [HttpGet]
        public async Task<ActionResult<TripDetailsOutputModel>> Get(GetTripQuery query)
            => await base.Send(query);

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TripListOutputModel>>> GetAll(GetAllTripsQuery query)
            => await base.Send(query);

        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteTripCommand command)
            => await base.Send(command);

        [HttpPut]
        public async Task<ActionResult> Update(UpdateTripCommand command)
            => await base.Send(command);
    }
}
