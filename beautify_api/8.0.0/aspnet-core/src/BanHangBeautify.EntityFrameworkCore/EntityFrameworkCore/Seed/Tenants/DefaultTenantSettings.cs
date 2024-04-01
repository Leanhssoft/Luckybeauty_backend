using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.EntityFrameworkCore.Seed.Tenants
{
    public class DefaultTenantSettings
    {
        private readonly SPADbContext _context;
        public DefaultTenantSettings(SPADbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            var tenancy = _context.Tenants.FirstOrDefault();
            int? tenantId = null;
            if (tenancy != null)
            {
                tenantId = tenancy.Id;
            }
            if (SPAConsts.MultiTenancyEnabled == false)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing
            //AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "SSOFT", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "false", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "true", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "admin@mydomain.com", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "smtp.gmail.com", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "587", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, "", tenantId);

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
