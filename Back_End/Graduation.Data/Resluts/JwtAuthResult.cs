using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Data.Resluts
{
    public class JwtAuthResult
    {
        public JwtAuthResult(string accessToken, RefreshToken refreshToken)
        {
            AccessToken = accessToken;
            this.refreshToken = refreshToken;
        }

        public string AccessToken { get; set; }
        public RefreshToken refreshToken { get; set; }
    }
   
}
