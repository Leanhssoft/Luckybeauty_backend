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
        private SettingManager _settingManager;
        public SettingsAppService(SettingManager settingManager)
        {
            _settingManager = settingManager;
        }
        public async Task<EmailSettingsEditDto> GetEmailSettingsAsync()
        {
            int tennatId = AbpSession.TenantId ?? 1;
            if (AbpSession.TenantId != null)
            {
                var smtpPassword = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Password, tennatId);
                return new EmailSettingsEditDto
                {
                    DefaultFromAddress = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, tennatId),
                    DefaultFromDisplayName =
                        await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromDisplayName, tennatId),
                    SmtpHost = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Host, tennatId),
                    SmtpPort = await _settingManager.GetSettingValueForTenantAsync<int>(EmailSettingNames.Smtp.Port, tennatId),
                    SmtpUserName = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.UserName, tennatId),
                    SmtpPassword = smtpPassword,
                    SmtpDomain = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Domain, tennatId),
                    SmtpEnableSsl = await _settingManager.GetSettingValueForTenantAsync<bool>(EmailSettingNames.Smtp.EnableSsl, tennatId),
                    SmtpUseDefaultCredentials =
                        await _settingManager.GetSettingValueForTenantAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials, tennatId)
                };
            }
            else
            {
                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                return new EmailSettingsEditDto
                {
                    DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                    DefaultFromDisplayName = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
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
        }
        [HttpPost]
        public async Task<ExecuteResultDto> UpdateEmailSettingsAsync(EmailSettingsEditDto settings)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            try
            {
                if (AbpSession.TenantId != null)
                {
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.DefaultFromAddress,
                settings.DefaultFromAddress);
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.DefaultFromDisplayName,
                        settings.DefaultFromDisplayName);
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.Smtp.Host, settings.SmtpHost);
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.Smtp.Port,
                        settings.SmtpPort.ToString(CultureInfo.InvariantCulture));
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.Smtp.UserName,
                        settings.SmtpUserName);
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.Smtp.Password,
                        settings.SmtpPassword);
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.Smtp.Domain, settings.SmtpDomain);
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.Smtp.EnableSsl,
                        settings.SmtpEnableSsl.ToString().ToLowerInvariant());
                    await _settingManager.ChangeSettingForTenantAsync(tenantId, EmailSettingNames.Smtp.UseDefaultCredentials,
                        settings.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
                }
                else
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
                }
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
