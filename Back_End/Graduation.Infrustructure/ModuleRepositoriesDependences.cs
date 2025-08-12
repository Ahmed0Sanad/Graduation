using Graduation.Infrustructure.Abstract;
using Graduation.Infrustructure.Context;
using Graduation.Infrustructure.Repositories;
using Graduation.Infrustructure.UnitOfWorks;
using Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;

namespace Graduation.Infrustructure
{
    public static class ModuleRepositoriesDependences
    {
        public static IServiceCollection RepositoryDependences(this IServiceCollection services , IConfiguration Configuration)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();
           // services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
 
            services.AddIdentity<AppUser, IdentityRole>(option =>
            {
                // Password settings.
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
              //  option.Password.RequireNonAlphanumeric = true;
               // option.Password.RequireUppercase = true;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = true;
               // option.SignIn.RequireConfirmedEmail = true;
            }
).
             AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            #region jwt
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = bool.Parse(Configuration["jwtSettings:ValidateIssuer"]),
                    ValidIssuer = Configuration["jwtSettings:Issuer"],
                    ValidateAudience = bool.Parse(Configuration["jwtSettings:ValidateAudience"]),
                    ValidAudience = Configuration["jwtSettings:Audience"],
                    ValidateIssuerSigningKey = bool.Parse(Configuration["jwtSettings:ValidateIssuerSigningKey"]),
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["jwtSettings:Secret"])),
                    ValidateLifetime = bool.Parse(Configuration["jwtSettings:ValidateLifetime"]),
                    ClockSkew = TimeSpan.FromMinutes(35),

                };
            }
               );



            #endregion



            return services;
        }
    }
}
