using Graduation.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Infrustructure.Specifications.AuthSpecifications
{
    public class RefreshTokenSpecification: Specification<UserRefreshToken>
    {
        public RefreshTokenSpecification( string accessToken, string refreshToken,string userId): 
            base(x => x.Token == accessToken && x.RefreshToken == refreshToken  && x.UserId == userId)
        {
                                              


        }
    }
}
