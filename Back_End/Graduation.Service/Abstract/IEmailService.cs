using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation.Data.Commons;

namespace Graduation.Service.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync( Email email );
    }
}
