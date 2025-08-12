using Graduation.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Graduation.Api.Base;
using Graduation.Core.Features.ApplicationUser.Commands.Models;
using Graduation.Core.Features.ApplicationUser.Queries.Models;
//using Data.AppMetaData;
namespace Graduation.Api.Controllers
{
   // [Authorize(Roles = "Admin,User")]
    public class ApplicationUserController : AppControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> Paginated([FromQuery] GetUserPaginationQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetStudentByID([FromRoute] string id)
        {
            return NewResult(await Mediator.Send(new GetUserByIdQuery(id)));
        }
        [HttpPut("EditUser")]
        public async Task<IActionResult> Edit([FromBody] EditUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize]
        [HttpDelete("DeleteUser/{id}")]
        
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            return NewResult(await Mediator.Send(new DeleteUserCommand(id)));
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
