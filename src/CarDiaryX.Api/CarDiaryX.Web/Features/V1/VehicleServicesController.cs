using CarDiaryX.Application.Features.V1.VehicleServices.Commands;
using CarDiaryX.Application.Features.V1.VehicleServices.OutputModels;
using CarDiaryX.Application.Features.V1.VehicleServices.Queries;
using CarDiaryX.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarDiaryX.Web.Features.V1
{
    [Version(1)]
    [Authorize]
    public class VehicleServicesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagingModel<VehicleServiceListingOutputModel>>> GetAll(string searchText = null, int page = 1)
            => await base.Send(new GetAllVehicleServicesQuery { Page = page < 1 ? 1 : page, SearchText = searchText });

        [HttpGet]
        [Route(ID_IDENTIFIER)]
        public async Task<ActionResult<VehicleServiceOutputModel>> Get(int id)
            => await base.Send(new GetVehicleServicesQuery { VehicleServicesId = id });

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AddVehicleServiceCommand command)
            => await base.Send(command);

        [HttpPost]
        public async Task<ActionResult> AddReview([FromBody] AddReviewCommand command)
            => await base.Send(command);

        [HttpGet]
        public async Task<ActionResult<PagingModel<ReviewOutputModel>>> GetAllReviews(int page = 1)
            => await base.Send(new GetAllReviewsQuery { Page = page < 1 ? 1 : page });

        // TODO: figure out how this should work
        [HttpPost]
        public async Task<ActionResult<object>> Report(object addObj)
            => await base.Send(null);
    }
}
