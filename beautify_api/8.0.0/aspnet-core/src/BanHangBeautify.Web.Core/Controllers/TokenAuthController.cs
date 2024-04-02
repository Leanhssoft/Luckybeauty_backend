using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.UI;
using BanHangBeautify.Authentication.External;
using BanHangBeautify.Authentication.JwtBearer;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Impersonation;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Models.TokenAuth;
using BanHangBeautify.MultiTenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BanHangBeautify.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : SPAControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly ICacheManager _cacheManager;
        private readonly UserManager _userManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IRepository<NS_NhanVien, Guid> _nhanSuRepository;
        private readonly AbpUserClaimsPrincipalFactory<User, Role> _claimsPrincipalFactory;
        private readonly IOptions<JwtBearerOptions> _jwtOptions;
        private readonly IImpersonationManager _impersonationManager;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            ICacheManager cacheManager,
            UserManager userManager,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            IRepository<NS_NhanVien, Guid> nhanSuRepository,
            AbpUserClaimsPrincipalFactory<User, Role> claimsPrincipalFactory,
            IOptions<JwtBearerOptions> jwtOptions,
            IImpersonationManager impersonationManager)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _cacheManager = cacheManager;
            _userManager = userManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _nhanSuRepository = nhanSuRepository;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _jwtOptions = jwtOptions;
            _impersonationManager = impersonationManager;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );
            var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
            var refreshToken = CreateRefreshToken(await CreateJwtClaims(loginResult.Identity,
                                                                        loginResult.User,
                                                                        tokenType: Authentication.JwtBearer.TokenType.RefreshToken,
                                                                        refreshTokenKey: SPAConsts.RefreshTokenValidityKey));
            var nhanSu = await _nhanSuRepository.FirstOrDefaultAsync(x => x.Id == loginResult.User.NhanSuId);
            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.token,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds
            };
        }
        [HttpPost]
        public async Task<ImpersonatedAuthenticateResultModel> ImpersonatedAuthenticate(string impersonationToken)
        {
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(impersonationToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new ImpersonatedAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds
            };
        }
        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);

            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    {
                        var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds
                        };
                    }
                case AbpLoginResultType.UnknownExternalLogin:
                    {
                        var newUser = await RegisterExternalUserAsync(externalUser);
                        if (!newUser.IsActive)
                        {
                            return new ExternalAuthenticateResultModel
                            {
                                WaitingForActivation = true
                            };
                        }

                        // Try to login again with newly registered user!
                        loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                        if (loginResult.Result != AbpLoginResultType.Success)
                        {
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                loginResult.Result,
                                model.ProviderKey,
                                GetTenancyNameOrNull()
                            );
                        }

                        var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));

                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds
                        };
                    }
                default:
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }
            }
        }
        [HttpPost]
        public async Task<RefreshTokenResult> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            if (!IsRefreshTokenValid(refreshToken, out var principal))
            {
                throw new ValidationException("Refresh token is not valid!");
            }

            try
            {
                var user = await _userManager.GetUserAsync(principal);

                if (user == null)
                {
                    throw new UserFriendlyException("Unknown user or user identifier");
                }

                principal = await _claimsPrincipalFactory.CreateAsync(user);
                var accessToken = CreateAccessToken(await CreateJwtClaims(principal.Identity as ClaimsIdentity, user));

                return await Task.FromResult(new RefreshTokenResult(
                    accessToken,
                    GetEncryptedAccessToken(accessToken),
                    (int)_configuration.AccessTokenExpiration.TotalSeconds)
                );
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ValidationException("Refresh token is not valid!", e);
            }
        }
        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                externalUser.Name,
                externalUser.Surname,
                externalUser.EmailAddress,
                externalUser.EmailAddress,
                Authorization.Users.User.CreateRandomPassword(),
                true
            );

            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalUser.Provider,
                    ProviderKey = externalUser.ProviderKey,
                    TenantId = user.TenantId
                }
            };

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.AccessTokenExpiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }


        private (string token, string key) CreateRefreshToken(IEnumerable<Claim> claims)
        {
            var claimsList = claims.ToList();
            return (CreateToken(claimsList, SPAConsts.RefreshTokenExpiration),
                claimsList.First(c => c.Type == SPAConsts.TokenValidityKey).Value);
        }
        private string CreateToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                signingCredentials: _configuration.SigningCredentials,
                expires: expiration == null ? (DateTime?)null : now.Add(expiration.Value)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private async Task<IEnumerable<Claim>> CreateJwtClaims(
            ClaimsIdentity identity, User user,
            TimeSpan? expiration = null,
            Authentication.JwtBearer.TokenType tokenType = Authentication.JwtBearer.TokenType.AccessToken,
            string refreshTokenKey = null)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            var tokenValidityKey = Guid.NewGuid().ToString();
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(SPAConsts.TokenValidityKey, tokenValidityKey),
                new Claim(SPAConsts.UserIdentifier, user.ToUserIdentifier().ToUserIdentifierString()),
                new Claim(SPAConsts.TokenType, tokenType.To<int>().ToString())
            });
            if (!string.IsNullOrEmpty(refreshTokenKey))
            {
                claims.Add(new Claim(SPAConsts.RefreshTokenValidityKey, refreshTokenKey));
            }

            if (!expiration.HasValue)
            {
                expiration = tokenType == Authentication.JwtBearer.TokenType.AccessToken
                    ? _configuration.AccessTokenExpiration
                    : _configuration.RefreshTokenExpiration;
            }
            var expirationDate = DateTime.UtcNow.Add(expiration.Value);

            await _cacheManager
                .GetCache(SPAConsts.TokenValidityKey)
                .SetAsync(tokenValidityKey, "", absoluteExpireTime: new DateTimeOffset(expirationDate));

            await _userManager.AddTokenValidityKeyAsync(
                user,
                tokenValidityKey,
                expirationDate
            );
            return claims;
        }
        private bool IsRefreshTokenValid(string refreshToken, out ClaimsPrincipal principal)
        {
            principal = null;

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidAudience = _configuration.Audience,
                    ValidIssuer = _configuration.Issuer,
                    IssuerSigningKey = _configuration.SecurityKey
                };

                foreach (var validator in _jwtOptions.Value.SecurityTokenValidators)
                {
                    if (!validator.CanReadToken(refreshToken))
                    {
                        continue;
                    }

                    try
                    {
                        principal = validator.ValidateToken(refreshToken, validationParameters, out _);

                        if (principal.Claims.FirstOrDefault(x => x.Type == SPAConsts.TokenType)?.Value ==
                            Authentication.JwtBearer.TokenType.RefreshToken.To<int>().ToString())
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(ex.ToString(), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.ToString(), ex);
            }

            return false;
        }
        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken);
        }
    }
}
