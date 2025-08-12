using Graduation.Core.Bases;
using Graduation.Core.Features.Authentication.Commands.Models;
using Graduation.Core.Resources;
using Graduation.Service.Abstract;
using Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Features.Authentication.Commands.Handlers
{
    public class Handler : ResponseHandler, IRequestHandler<SignInCommand, Response<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthenticationService _authenticationService;

        public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthenticationService authenticationService  )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._authenticationService = authenticationService;
        }
        public async Task<Response<string>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            //Check if user is exist or not
            var user = await _userManager.FindByEmailAsync(request.Email);
            //Return The UserName Not Found
            if (user == null) return BadRequest<string>("Email Not vaild");
            //try To Sign in 
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            //if Failed Return Passord is wrong
            if (!signInResult.Succeeded) return BadRequest<string>("Password Not vaild");
            //confirm email
            //if (!user.EmailConfirmed)
            //    return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.EmailNotConfirmed]);
            //Generate Token
            var result = await _authenticationService.GetJWTToken(user);
            //return Token 
            return Success(result);
        }
    }
}
