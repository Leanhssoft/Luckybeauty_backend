using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using BanHangBeautify.Configuration.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BanHangBeautify.Configuration.Settings
{
    public interface ISettings
    {
        Task<EmailSettingsEditDto> GetEmailSettingsAsync();
    }
}
