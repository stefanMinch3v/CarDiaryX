using CarDiaryX.Application.Features.V1.Trips.Commands;
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
        public async Task<IActionResult> Add(AddNewTripCommand command)
            => await base.Send(command);

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //    => base.Send(null);

        //[HttpDelete]
        //public async Task<IActionResult> Delete()
        //    => base.Send(null);

        //[HttpPut]
        //public async Task<IActionResult> Update()
        //    => base.Send(null);
    }
}
