using Graduation.Core.Behaviors;
using Graduation.Core.Mapping.ApplicationUser;
using Graduation.Core.Mapping.Students;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Graduation.Core
{
    public static class ModuleCoreDependences
    {
        public static IServiceCollection CoreDependences(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Replace this line:
            // services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // With this line:
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
          
            // get validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            // 
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
