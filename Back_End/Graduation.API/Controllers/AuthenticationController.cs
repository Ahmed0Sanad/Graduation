using Graduation.Core.Features.Authentication.Commands.Models;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation.API.Controllers
{

    public class AuthenticationController : AppControllerBase
    {

        [HttpPost("SignIn")]
        public async Task<IActionResult> Create([FromQuery] SignInCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        //[HttpPost("RefreshToken")]
        //public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        //{
        //    var response = await Mediator.Send(command);
        //    return NewResult(response);
        //}
        //[HttpGet("ValidateToken")]
        //public async Task<IActionResult> ValidateToken([FromQuery] AuthorizeUserQuery query)
        //{
        //    var response = await Mediator.Send(query);
        //    return NewResult(response);
        //}
        //[HttpGet("ConfirmEmail")]
        //public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
        //{
        //    var response = await Mediator.Send(query);
        //    return NewResult(response);
        //}
        //[HttpPost("SendResetPasswordCode")]
        //public async Task<IActionResult> SendResetPassword([FromQuery] SendResetPasswordCommand command)
        //{
        //    var response = await Mediator.Send(command);
        //    return NewResult(response);
        //}
        //[HttpGet("ConfirmResetPasswordCode")]
        //public async Task<IActionResult> ConfirmResetPassword([FromQuery] ConfirmResetPasswordQuery query)
        //{
        //    var response = await Mediator.Send(query);
        //    return NewResult(response);
        //}
        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand command)
        //{
        //    var response = await Mediator.Send(command);
        //    return NewResult(response);
        //}
    }
}
