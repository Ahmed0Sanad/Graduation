using Azure.Core;
using Graduation.Data.Commons;
using Graduation.Data.Entities.Identity;
using Graduation.Service.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Service.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;
        private readonly IEmailService _emailService;

        public ApplicationUserService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor , IUrlHelper urlHelper , IEmailService emailService)
        {
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
            this._urlHelper = urlHelper;
            this._emailService = emailService;
        }
        public async Task<bool> AddUserAsync(AppUser user, string password)
        {
            var createResult = await _userManager.CreateAsync(user, password);
            if (createResult.Succeeded) {

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var resquestAccessor = _httpContextAccessor.HttpContext.Request;

                var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host + _urlHelper.Action("ConfirmEmail", "Authentication", new { userId = user.Id, code = code });
                


            Email request = new Email()
                {
                    To = user.Email,
                    Subject = "Email Confirmation",
                    IsBodyHtml = true,
                    Body =
                $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"" />
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
  <title>Email Confirmation</title>
  <style>
    /* Client-resets that survive most providers */
    body,table,td,a {{ -webkit-text-size-adjust:100%; -ms-text-size-adjust:100%; }}
    table,td {{ mso-table-lspace:0pt; mso-table-rspace:0pt; }}
    img {{ -ms-interpolation-mode:bicubic; border:0; outline:none; text-decoration:none; }}
    body {{ margin:0; padding:0; width:100%!important; height:100%!important; }}
    /* Container */
    .wrapper {{ width:100%; background:#f6f8fb; padding:24px 0; }}
    .container {{ max-width:560px; margin:0 auto; background:#ffffff; border-radius:14px; overflow:hidden; box-shadow:0 4px 18px rgba(0,0,0,0.06); }}
    .header {{ padding:20px 28px; background:#0f172a; color:#ffffff; font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif; }}
    .header h1 {{ margin:0; font-size:20px; font-weight:600; }}
    .content {{ padding:28px; font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif; color:#0f172a; }}
    .content h2 {{ margin:0 0 12px; font-size:18px; font-weight:700; }}
    .content p {{ margin:0 0 14px; line-height:1.55; font-size:15px; }}
    .btn {{ display:inline-block; padding:12px 20px; text-decoration:none; border-radius:10px; background:#2563eb; color:#ffffff!important; font-weight:600; }}
    .note {{ font-size:12px; color:#475569; margin-top:16px; }}
    .divider {{ height:1px; background:#e2e8f0; margin:20px 0; }}
    .footer {{ padding:18px 28px; font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif; color:#64748b; font-size:12px; }}
    /* Dark mode hint (best effort) */
    @media (prefers-color-scheme: dark) {{
      .wrapper {{ background:#0b1220; }}
      .container {{ background:#111827; }}
      .header {{ background:#0b1220; }}
      .content {{ color:#e5e7eb; }}
      .note {{ color:#9ca3af; }}
      .divider {{ background:#1f2937; }}
    }}
    /* Mobile */
    @media only screen and (max-width:600px) {{
      .content, .header, .footer {{ padding-left:20px; padding-right:20px; }}
    }}
  </style>
</head>
<body>
  <div class=""wrapper"">
    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"">
      <tr>
        <td align=""center"">
          <div class=""container"">
            <div class=""header"">
              <h1>Email Confirmation</h1>
            </div>
            <div class=""content"">
              <h2>Hello{(string.IsNullOrWhiteSpace(user.FName) ? "" : " " + System.Net.WebUtility.HtmlEncode(user.FName))},</h2>
              <p>Thanks for signing up! Please confirm your email address to finish setting up your account.</p>
              <p style=""margin:22px 0;"">
                <a class=""btn"" href=""{System.Net.WebUtility.HtmlEncode(returnUrl)}"" target=""_blank"" rel=""noopener"">
                  Confirm Email
                </a>
              </p>
              <p class=""note"">This link will expire after a short period for security.</p>

              <div class=""divider""></div>

              <p>If the button doesn’t work, copy and paste this link into your browser:</p>
              <p style=""word-break:break-all;"">
                <a href=""{System.Net.WebUtility.HtmlEncode(returnUrl)}"" target=""_blank"" rel=""noopener"">
                  {System.Net.WebUtility.HtmlEncode(returnUrl)}
                </a>
              </p>

              <p class=""note"">If you didn’t create an account, you can safely ignore this email.</p>
            </div>
            <div class=""footer"">
              © {DateTime.UtcNow:yyyy} Your Company — All rights reserved.
            </div>
          </div>
        </td>
      </tr>
    </table>
  </div>
</body>
</html>"
                };

                var sendEmail = await _emailService.SendEmailAsync(request);
                if (sendEmail)
                {
                    return true;

                }
                else
                {
                    return false;
                }

            }
            else { return false; }

        }
    }
}
