using Graduation.Core.Features.ApplicationUser.Commands.Models;
using Graduation.Data.Entities.Identity;

namespace Graduation.Core.Mapping.ApplicationUser
{
    public partial class ApplicationUserProfile
    {
        public void UpdateUserMapping()
        {
            CreateMap<EditUserCommand, AppUser>()
                .ForMember(dest => dest.FName, org => org.MapFrom(org=>org.FullName)).ReverseMap();
            ;
        }
    }
}
