using Graduation.Core.Bases;
using Graduation.Data.Resluts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Features.Authentication.Commands.Models
{
    public class SignInCommand: IRequest<Response<JwtAuthResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
       
    }
}
