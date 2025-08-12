using Graduation.Core.Features.ApplicationUser.Queries.Results;

using Data.Entities;

namespace Graduation.Core.Mapping.ApplicationUser
{
    public partial class ApplicationUserProfile
    {
        public void GetUserPaginationMapping()
        {
            CreateMap<AppUser, GetUserPaginationReponse>()
                .ForMember(dest => dest.FullName, org => org.MapFrom(org=>org.FName)).ReverseMap();
            ;

        }
    }
}