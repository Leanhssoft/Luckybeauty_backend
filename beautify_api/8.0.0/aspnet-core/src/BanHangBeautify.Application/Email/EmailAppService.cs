using Abp.Dependency;
using Abp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Email
{
    public class EmailAppService: SPAAppServiceBase
    {
        private readonly IEmailSender _emailSender;
        public EmailAppService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task SendEmail(string emailAddressReciver,string titleEmail,string bodyEmail)
        {
            await _emailSender.SendAsync(emailAddressReciver, titleEmail, bodyEmail, false) ;
        }
    }
}
