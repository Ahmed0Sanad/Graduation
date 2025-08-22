using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Data.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FName { get; set; }
        public string Address { get; set; }
        public string? Country { get; set; }
  
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
