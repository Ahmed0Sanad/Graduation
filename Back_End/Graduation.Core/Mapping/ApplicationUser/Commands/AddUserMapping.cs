using Graduation.Core.Features.ApplicationUser.Commands.Models;
using Graduation.Core.Features.Students.Quaries.Results;
using Graduation.Data.Entities.Identity;

namespace Graduation.Core.Mapping.ApplicationUser
{
    public partial class ApplicationUserProfile
    {
        public void AddUserMapping()
        {
            CreateMap<AppUser, AddUserCommand>()
                .ForMember(dest => dest.FullName, org => org.MapFrom(org=>org.FName))
                 .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                 .ForMember(dest => dest.Password, opt => opt.Ignore()).ReverseMap()




                 ;


        }
    }
}