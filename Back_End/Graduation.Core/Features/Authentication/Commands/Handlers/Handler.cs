using Graduation.Core.Bases;
using Graduation.Core.Features.Authentication.Commands.Models;
using Graduation.Core.Resources;
using Graduation.Data.Entities.Identity;
using Graduation.Data.Resluts;
using Graduation.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Features.Authentication.Commands.Handlers
{
    public class Handler : ResponseHandler, 
        IRequestHandler<SignInCommand, Response<JwtAuthResult>>,
        IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>

    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        

        public Handler(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, 
            IAuthenticationService authenticationService
             
            )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._authenticationService = authenticationService;
           
        }
        public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            //Check if user is exist or not
            var user = await _userManager.FindByEmailAsync(request.Email);
            //Return The UserName Not Found
            if (user == null) return BadRequest<JwtAuthResult>("Email Not vaild");
            //try To Sign in 
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            //if Failed Return Passord is wrong
            if (!signInResult.Succeeded) return BadRequest<JwtAuthResult>("Password Not vaild");
            //confirm email
            //if (!user.EmailConfirmed)
            //    return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.EmailNotConfirmed]);
            //Generate Token
            var result = await _authenticationService.GetJWTToken(user);
            //return Token 
            return Success(result);
        }

        public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtToken = _authenticationService.ReadJWTToken(request.AccessToken);
            var userIdAndExpireDate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
            switch (userIdAndExpireDate)
            {
                case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResult>();
                case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResult>();
                case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResult>();
                case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResult>();
            }
            var (userId, expiryDate) = userIdAndExpireDate;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound<JwtAuthResult>();
            }
            var result = await _authenticationService.GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);
            return Success(result);

        }
    }
}
