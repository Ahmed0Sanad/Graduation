using Graduation.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Service.Abstract
{
    public interface IApplicationUserService
    {
        Task<bool> AddUserAsync(AppUser user, string password);
        //Task<IdentityResult> EditUserAsync(AppUser user);
        //Task<IdentityResult> DeleteUserAsync(AppUser user);
        //Task<IdentityResult> ChangeUserPasswordAsync(AppUser user, string newPassword);
    }
}
