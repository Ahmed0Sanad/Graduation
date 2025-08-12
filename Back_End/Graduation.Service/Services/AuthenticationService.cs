using Graduation.Service.Abstract;
using Data.Entities;
using Data.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<AppUser> _userManager;

        public AuthenticationService(IConfiguration configuration , IOptions<JwtSettings> jwtSettings , UserManager<AppUser> userManager)
        {
            this._configuration = configuration;
            this._jwtSettings = jwtSettings.Value;
            this._userManager = userManager;
        }
        public async Task<string> GetJWTToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var claims = new List<Claim>()
            {
              new Claim(ClaimTypes.Name, user.FName),
              new Claim(ClaimTypes.Email , user.Email)
            };
            var UserRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: credentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);


        }
  




        public Task<string> ConfirmEmail(int? userId, string? code)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConfirmResetPassword(string Code, string Email)
        {
            throw new NotImplementedException();
        }

       
        public JwtSecurityToken ReadJWTToken(string accessToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> ResetPassword(string Email, string Password)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendResetPasswordCode(string Email)
        {
            throw new NotImplementedException();
        }

        public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> ValidateToken(string AccessToken)
        {
            throw new NotImplementedException();
        }
    }
}
