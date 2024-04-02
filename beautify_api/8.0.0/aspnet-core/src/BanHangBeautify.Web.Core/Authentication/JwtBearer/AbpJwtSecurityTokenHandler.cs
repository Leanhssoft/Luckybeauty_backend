using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Threading;
using Abp.Timing;
using Abp.UI;
using Abp;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.Authorization.Users;
using Abp.Authorization;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Delegation;

namespace BanHangBeautify.Authentication.JwtBearer
{
    public class AbpJwtSecurityTokenHandler : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public AbpJwtSecurityTokenHandler()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var cacheManager = IocManager.Instance.Resolve<ICacheManager>();
            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            if (!HasAccessTokenType(principal))
            {
                throw new SecurityTokenException("invalid token type");
            }

            AsyncHelper.RunSync(() => ValidateSecurityStampAsync(principal));

            var tokenValidityKeyClaim = principal.Claims.First(c => c.Type == SPAConsts.TokenValidityKey);
            if (TokenValidityKeyExistsInCache(tokenValidityKeyClaim, cacheManager))
            {
                return principal;
            }

            var userIdentifierString = principal.Claims.First(c => c.Type == SPAConsts.UserIdentifier);
            var userIdentifier = UserIdentifier.Parse(userIdentifierString.Value);

            if (!ValidateTokenValidityKey(tokenValidityKeyClaim, userIdentifier))
            {
                throw new SecurityTokenException("invalid");
            }

            var tokenAuthConfiguration = IocManager.Instance.Resolve<TokenAuthConfiguration>();

            cacheManager.GetCache(SPAConsts.TokenValidityKey).Set(
                tokenValidityKeyClaim.Value, "",
                absoluteExpireTime: new DateTimeOffset(Clock.Now.AddMinutes(tokenAuthConfiguration.AccessTokenExpiration.TotalMinutes))
            );

            return principal;
        }

        private bool ValidateTokenValidityKey(Claim tokenValidityKeyClaim, UserIdentifier userIdentifier)
        {
            bool isValid;

            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = unitOfWorkManager.Object.Begin())
                {
                    using (unitOfWorkManager.Object.Current.SetTenantId(userIdentifier.TenantId))
                    {
                        using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
                        {
                            var userManagerObject = userManager.Object;
                            var user = userManagerObject.GetUserById(userIdentifier.UserId);
                            isValid = AsyncHelper.RunSync(() =>
                                userManagerObject.IsTokenValidityKeyValidAsync(user, tokenValidityKeyClaim.Value));

                            uow.Complete();
                        }
                    }
                }
            }

            return isValid;
        }

        private static bool TokenValidityKeyExistsInCache(Claim tokenValidityKeyClaim, ICacheManager cacheManager)
        {
            var tokenValidityKeyInCache = cacheManager
                .GetCache(SPAConsts.TokenValidityKey)
                .GetOrDefault(tokenValidityKeyClaim.Value);

            return tokenValidityKeyInCache != null;
        }

        private static async Task ValidateSecurityStampAsync(ClaimsPrincipal principal)
        {
            ValidateUserDelegation(principal);

            using (var securityStampHandler = IocManager.Instance.ResolveAsDisposable<IJwtSecurityStampHandler>())
            {
                if (!await securityStampHandler.Object.Validate(principal))
                {
                    throw new SecurityTokenException("invalid");
                }
            }
        }

        private bool HasAccessTokenType(ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == SPAConsts.TokenType)?.Value ==
                   TokenType.AccessToken.To<int>().ToString();
        }

        private static void ValidateUserDelegation(ClaimsPrincipal principal)
        {
            var _userDelegationConfiguration = IocManager.Instance.Resolve<IUserDelegationConfiguration>();
            if (!_userDelegationConfiguration.IsEnabled)
            {
                return;
            }

            var impersonatorTenant = principal.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorTenantId);
            var user = principal.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.UserId);
            var impersonatorUser = principal.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorUserId);

            if (impersonatorUser == null || user == null)
            {
                return;
            }

            var impersonatorTenantId = impersonatorTenant == null ? null :
                impersonatorTenant.Value.IsNullOrEmpty() ? (int?)null : Convert.ToInt32(impersonatorTenant.Value);
            var sourceUserId = Convert.ToInt64(user.Value);
            var impersonatorUserId = Convert.ToInt64(impersonatorUser.Value);

            using (var _permissionChecker = IocManager.Instance.ResolveAsDisposable<PermissionChecker>())
            {
                if (_permissionChecker.Object.IsGranted(new UserIdentifier(impersonatorTenantId, impersonatorUserId),
                    PermissionNames.Pages_Administration_Users_Impersonation))
                {
                    return;
                }
            }

            using (var userDelegationManager = IocManager.Instance.ResolveAsDisposable<IUserDelegationManager>())
            {
                var hasActiveDelegation =
                    userDelegationManager.Object.HasActiveDelegation(sourceUserId, impersonatorUserId);

                if (!hasActiveDelegation)
                {
                    throw new UserFriendlyException("ThereIsNoActiveUserDelegationBetweenYourUserAndCurrentUser");
                }
            }
        }
    }
}
