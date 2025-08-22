using Azure.Core;
using Data.Helper;
using Graduation.Data.Entities.Identity;
using Graduation.Data.Resluts;
using Graduation.Infrustructure.Specifications.AuthSpecifications;
using Graduation.Infrustructure.UnitOfWorks;
using Graduation.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
      
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(IOptions<JwtSettings> jwtSettings , UserManager<AppUser> userManager , IUnitOfWork unitOfWork)
        {
        
            this._jwtSettings = jwtSettings.Value;
            this._userManager = userManager;
            this._unitOfWork = unitOfWork;
        }
        public async Task<JwtAuthResult> GetJWTToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
      
     

            var (jwtToken, accessToken) = await GenerateJWTToken(user);

            var refreshToken = new RefreshToken
            {
                ExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                UserName = user.UserName,
                TokenString = GenerateRefreshToken()
            };

            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                IsUsed = true,
                IsRevoked = false,
                JwtId = jwtToken.Id,
                RefreshToken = refreshToken.TokenString,
                Token = accessToken,
                UserId = user.Id
            };
            await _unitOfWork.Repository<UserRefreshToken>().AddAsync(userRefreshToken);
            await _unitOfWork.CompleteAsync ();

            var response = new JwtAuthResult( accessToken , refreshToken);
            
            return response;


        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<JwtAuthResult> GetRefreshToken(AppUser user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
        {
            var (jwtSecurityToken, newToken) = await GenerateJWTToken(user);

            var refreshTokenResult = new RefreshToken();

            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name )?.Value;
            refreshTokenResult.UserName = userName;
            refreshTokenResult.TokenString = refreshToken;

            refreshTokenResult.ExpireAt = (DateTime)expiryDate;

            var response = new JwtAuthResult(newToken, refreshTokenResult);

            return response;
        }

        private async Task<(JwtSecurityToken, string)> GenerateJWTToken(AppUser user)
        {
            var claims = new List<Claim>()
            {
              new Claim(ClaimTypes.Name, user.FName),
              new Claim(ClaimTypes.Email , user.Email),
              new Claim("Id", user.Id), 
            };

            var UserRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate ),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }
        public async Task<string> ConfirmEmail(string? userId, string? code)
        {
            if (userId == null || code == null)
                return "ErrorWhenConfirmEmail";
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var confirmEmail = await _userManager.ConfirmEmailAsync(user, code);
            if (!confirmEmail.Succeeded)
                return "ErrorWhenConfirmEmail";
            return "Success";
        }

        public Task<string> ConfirmResetPassword(string Code, string Email)
        {
            throw new NotImplementedException();
        }

       
        public JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }

        public Task<string> ResetPassword(string Email, string Password)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendResetPasswordCode(string Email)
        {
            throw new NotImplementedException();
        }

        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                return ("AlgorithmIsWrong", null);
            }
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("TokenIsNotExpired", null);
            }

            //Get User
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            var spec = new RefreshTokenSpecification (AccessToken, RefreshToken, userId);
            var userRefreshToken = await _unitOfWork.Repository<UserRefreshToken>(). GetFirstOrDefaultSpecAsync (spec);
            if (userRefreshToken == null)
            {
                return ("RefreshTokenIsNotFound", null);
            }

            if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;
                 _unitOfWork.Repository<UserRefreshToken>()  .Update(userRefreshToken);
                return ("RefreshTokenIsExpired", null);
            }
            var expirydate = userRefreshToken.ExpiryDate;
            return (userId, expirydate);
        }

        public async Task<string> ValidateToken(string AccessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
            };
            try
            {
                var validator = handler.ValidateToken(AccessToken, parameters, out SecurityToken validatedToken);

                if (validator == null)
                {
                    return "InvalidToken";
                }

                return "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
