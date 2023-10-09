using Abp.Authorization;
using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using BanHangBeautify.Configuration.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Configuration.Settings
{
    [AbpAuthorize]
    public class SettingsAppService: SPAAppServiceBase
    {
        public async Task<EmailSettingsEditDto> GetEmailSettingsAsync()
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

            return new EmailSettingsEditDto
            {
                DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName =
                    await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                SmtpPassword = smtpPassword,
                SmtpDomain = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials =
                    await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)
            };
        }
        [HttpPost]
        public async Task<ExecuteResultDto> UpdateEmailSettingsAsync(EmailSettingsEditDto settings)
        {
            try
            {
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress,
                settings.DefaultFromAddress);
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName,
                    settings.DefaultFromDisplayName);
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, settings.SmtpHost);
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port,
                    settings.SmtpPort.ToString(CultureInfo.InvariantCulture));
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName,
                    settings.SmtpUserName);
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password,
                    settings.SmtpPassword);
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, settings.SmtpDomain);
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl,
                    settings.SmtpEnableSsl.ToString().ToLowerInvariant());
                await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials,
                    settings.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
                return new ExecuteResultDto()
                {
                    Message = "Sửa dữ liệu thành công !",
                    Status = "success"
                };
            }
            catch (Exception ex)
            {

                return new ExecuteResultDto()
                {
                    Message = "Có lỗi xảy ra vui lòng thử lại sau !",
                    Detail = ex.Message.ToString(),
                    Status = "error"
                };
            }
            
        }

    }
}
