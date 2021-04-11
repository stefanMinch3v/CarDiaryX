using CarDiaryX.Application.Features.V1.Identity.Commands;
using CarDiaryX.Application.Features.V1.Identity.OutputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarDiaryX.Web.Features.V1
{
    [Version(1)]
    public class IdentityController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Register([FromBody]RegisterUserCommand command)
            => await this.Send(command);

        [HttpPost]
        public async Task<ActionResult<LoginOutputModel>> Login([FromBody]LoginUserCommand command)
            => await this.Send(command);

        // To test if JWT is working
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Get()
            => this.Ok(this.User.Identity.Name);
    }
}
