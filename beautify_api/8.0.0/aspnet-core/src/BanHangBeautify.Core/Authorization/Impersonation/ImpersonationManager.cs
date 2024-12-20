﻿using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using BanHangBeautify.Authorization.Impersonation.Dto;
using BanHangBeautify.Authorization.Users;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Authorization.Impersonation
{
    public class ImpersonationManager : BanHangBeautifyDomainServiceBase, IImpersonationManager
    {
        public IAbpSession AbpSession { get; set; }
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICacheManager _cacheManager;
        private readonly UserManager _userManager;
        private readonly UserClaimsPrincipalFactory _principalFactory;

        public ImpersonationManager(
            ICacheManager cacheManager,
            UserManager userManager,
            UserClaimsPrincipalFactory principalFactory,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _userManager = userManager;
            _principalFactory = principalFactory;
            _unitOfWorkManager = unitOfWorkManager;
            AbpSession = NullAbpSession.Instance;
        }
        [UnitOfWork]
        public async Task<UserAndIdentity> GetImpersonatedUserAndIdentity(string impersonationToken)
        {
            var cacheItem = await _cacheManager.GetImpersonationCache().GetOrDefaultAsync(impersonationToken);
            if (cacheItem == null)
            {
                throw new UserFriendlyException(L("ImpersonationTokenErrorMessage"));
            }

            
            using (_unitOfWorkManager.Current.SetTenantId(cacheItem.TargetTenantId))
            {
                //CheckCurrentTenant(cacheItem.TargetTenantId);

                //Get the user from tenant
                var user = await _userManager.FindByIdAsync(cacheItem.TargetUserId.ToString());

                //Create identity
                var identity = await GetClaimsIdentityFromCache(user, cacheItem);

                if (!cacheItem.IsBackToImpersonator)
                {
                    //Add claims for audit logging
                    if (cacheItem.ImpersonatorTenantId.HasValue)
                    {
                        identity.AddClaim(new Claim(AbpClaimTypes.ImpersonatorTenantId, cacheItem.ImpersonatorTenantId.Value.ToString(CultureInfo.InvariantCulture)));
                    }

                    identity.AddClaim(new Claim(AbpClaimTypes.ImpersonatorUserId, cacheItem.ImpersonatorUserId.ToString(CultureInfo.InvariantCulture)));
                }

                //Remove the cache item to prevent re-use
                await _cacheManager.GetImpersonationCache().RemoveAsync(impersonationToken);


                return new UserAndIdentity(user, identity);
            }
        }

        private async Task<ClaimsIdentity> GetClaimsIdentityFromCache(User user, ImpersonationCacheItem cacheItem)
        {
            var identity = (ClaimsIdentity)(await _principalFactory.CreateAsync(user)).Identity;

            if (!cacheItem.IsBackToImpersonator)
            {
                //Add claims for audit logging
                if (cacheItem.ImpersonatorTenantId.HasValue)
                {
                    identity.AddClaim(new Claim(AbpClaimTypes.ImpersonatorTenantId,
                        cacheItem.ImpersonatorTenantId.Value.ToString(CultureInfo.InvariantCulture)));
                }

                identity.AddClaim(new Claim(AbpClaimTypes.ImpersonatorUserId,
                    cacheItem.ImpersonatorUserId.ToString(CultureInfo.InvariantCulture)));
            }

            return identity;
        }

        public Task<string> GetImpersonationToken(long userId, int? tenantId)
        {
            if (AbpSession.ImpersonatorUserId.HasValue)
            {
                throw new UserFriendlyException(L("CascadeImpersonationErrorMessage"));
            }

            if (AbpSession.TenantId.HasValue)
            {
                if (!tenantId.HasValue)
                {
                    throw new UserFriendlyException(L("FromTenantToHostImpersonationErrorMessage"));
                }

                if (tenantId.Value != AbpSession.TenantId.Value)
                {
                    throw new UserFriendlyException(L("DifferentTenantImpersonationErrorMessage"));
                }
            }

            return GenerateImpersonationTokenAsync(tenantId, userId, false);
        }

        public Task<string> GetBackToImpersonatorToken()
        {
            if (!AbpSession.ImpersonatorUserId.HasValue)
            {
                throw new UserFriendlyException(L("NotImpersonatedLoginErrorMessage"));
            }

            return GenerateImpersonationTokenAsync(AbpSession.ImpersonatorTenantId, AbpSession.ImpersonatorUserId.Value, true);
        }

        private void CheckCurrentTenant(int? tenantId)
        {
            if (AbpSession.TenantId != tenantId)
            {
                throw new Exception($"Current tenant is different than given tenant. AbpSession.TenantId: {AbpSession.TenantId}, given tenantId: {tenantId}");
            }
        }

        private async Task<string> GenerateImpersonationTokenAsync(int? tenantId, long userId, bool isBackToImpersonator)
        {
            //Create a cache item
            var cacheItem = new ImpersonationCacheItem(
                tenantId,
                userId,
                isBackToImpersonator
            );

            if (!isBackToImpersonator)
            {
                cacheItem.ImpersonatorTenantId = AbpSession.TenantId;
                cacheItem.ImpersonatorUserId = AbpSession.GetUserId();
            }

            //Create a random token and save to the cache
            var token = Guid.NewGuid().ToString();

            await _cacheManager
                .GetImpersonationCache()
                .SetAsync(token, cacheItem, TimeSpan.FromMinutes(1));

            return token;
        }
    }
}
