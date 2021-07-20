using CarDiaryX.Application.Features.V1.Identity.Commands;
using CarDiaryX.Application.Features.V1.Identity.OutputModels;
using CarDiaryX.Application.Features.V1.Identity.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarDiaryX.Web.Features.V1
{
    [Version(1)]
    [Authorize]
    public class IdentityController : ApiControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterUserCommand command)
            => await base.Send(command);

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<LoginOutputModel>> Login([FromBody] LoginUserCommand command)
            => await base.Send(command);

        [HttpPut]
        public async Task<ActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
            => await base.Send(command);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] DeleteUserCommand command)
            => await base.Send(command);

        [HttpGet]
        public async Task<ActionResult<UserDetailsOutputModel>> UserDetails()
            => await base.Send(new GetUserDetailsQuery());

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateUserDetailsCommand command)
            => await base.Send(command);

        // To test if JWT is working
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetEmailTest()
            => this.Ok(this.User.Identity.Name);
    }
}
