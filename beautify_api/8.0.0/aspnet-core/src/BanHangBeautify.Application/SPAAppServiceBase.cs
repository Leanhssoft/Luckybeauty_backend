using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class SPAAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected SPAAppServiceBase()
        {
            LocalizationSourceName = SPAConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.TenantId??1);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
