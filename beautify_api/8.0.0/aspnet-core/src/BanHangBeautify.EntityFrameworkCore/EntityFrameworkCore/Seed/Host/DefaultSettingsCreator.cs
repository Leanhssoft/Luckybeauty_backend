﻿using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace BanHangBeautify.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly SPADbContext _context;

        public DefaultSettingsCreator(SPADbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            if (SPAConsts.MultiTenancyEnabled == false)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "ssoft@ssoft.vn", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "SSOFT", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "false", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "true", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "ducmanh.ssoft@gmail.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "ovikqyilsuxzdkde", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "smtp.gmail.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "465", tenantId);// 465 cho SSL ,587 cho TLS
            AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, "", tenantId);

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "vi", tenantId);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
