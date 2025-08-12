using Graduation.Service.Abstract;
using Graduation.Service.Services;
using Data.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Graduation.Service
{
    public static class ModuleServiceDependences
    {

        public static IServiceCollection ServiceDependences(this IServiceCollection services)
        {
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();




            return services;
        }
    }
}
