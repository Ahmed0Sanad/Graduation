
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string FName { get; set; }
        public string Address { get; set; }
        public string? Country { get; set; }
    }
}
