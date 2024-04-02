using Abp.Authorization;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using BanHangBeautify.Authorization.Accounts.Dto;
using BanHangBeautify.Authorization.Impersonation;
using BanHangBeautify.Authorization.Impersonation.Dto;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.SeedData;
using BanHangBeautify.Url;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Authorization.Accounts
{
    public class AccountAppService : SPAAppServiceBase, IAccountAppService, ITransientDependency
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";
        public IAbpSession _abpSession { get; set; }
        private ISession _session { set; get; }
        private readonly IConfiguration _config;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IUserEmailer _userEmailer;
        public IAppUrlService AppUrlService { get; set; }
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly AbpZeroDbMigrator _migrator;
        private readonly ISeedDataAppService _seedDataEntities;
        private readonly IImpersonationManager _impersonationManager;
        public AccountAppService(
            UserRegistrationManager userRegistrationManager,
            IAbpSession session,
            IUserEmailer userEmailer,
            IAppUrlService appUrlService,
            IUnitOfWorkManager unitOfWorkManager,
            AbpZeroDbMigrator migrator,
            ISeedDataAppService seedDataEntities,
            IImpersonationManager impersonationManager
            )
        {
            _userRegistrationManager = userRegistrationManager;
            _abpSession = session;
            _userEmailer = userEmailer;
            AppUrlService = appUrlService;
            _unitOfWorkManager = unitOfWorkManager;
            _migrator = migrator;
            _seedDataEntities = seedDataEntities;
            _impersonationManager = impersonationManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            //if (string.IsNullOrEmpty(input.TenancyName)) // nếu input.TenancyName null hoặc empty thì là host
            //{
            //    return new IsTenantAvailableOutput(TenantAvailabilityState.Available, null);
            //}
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive || (tenant.SubscriptionEndDate != null && tenant.SubscriptionEndDate < DateTime.Now))
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            // migration
            if (tenant.Id == 1)
            {
                _migrator.CreateOrMigrateForHost();
            }
            else
            {
                _migrator.CreateOrMigrateForTenant(tenant);
            }

            _seedDataEntities.InnitData(tenant.Id);

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }
        public async Task<bool> SendPasswordResetCode(SendPasswordResetCodeInput input)
        {
            bool result = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(input.TenantId))
                {
                    await CurrentUnitOfWork.SaveChangesAsync();
                    var user = await GetUserByChecking(input.EmailAddress);
                    user.SetNewPasswordResetCode();
                    var url = AppUrlService.CreatePasswordResetUrlFormat(input.TenantId);
                    await _userEmailer.SendPasswordResetLinkAsync(
                        user,
                        url
                        );
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        {
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(input.TenantId))
                {
                    await CurrentUnitOfWork.SaveChangesAsync();
                    var user = await UserManager.GetUserByIdAsync(input.UserId);
                    if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != input.ResetCode)
                    {
                        return new ResetPasswordOutput
                        {
                            CanLogin = false,
                            UserName = user.UserName,
                            Message = L("InvalidPasswordResetCode")
                        };
                    }


                    await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                    CheckErrors(await UserManager.ChangePasswordAsync(user, input.Password));
                    user.PasswordResetCode = null;
                    user.IsEmailConfirmed = true;

                    await UserManager.UpdateAsync(user);

                    return new ResetPasswordOutput
                    {
                        CanLogin = user.IsActive,
                        UserName = user.UserName,
                        Message = "Thay đổi mật khẩu thành công"
                    };
                }
            }
            catch (Exception)
            {
                return new ResetPasswordOutput
                {
                    CanLogin = false,
                    UserName = "",
                    Message = "Có lỗi xảy ra vui lòng thử lại sau!"
                };
            }
        }

        public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        {
            var user = await GetUserByChecking(input.EmailAddress);
            user.SetNewEmailConfirmationCode();
            await _userEmailer.SendEmailActivationLinkAsync(
            user,
                AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
            );
        }

        public async Task ActivateEmail(ActivateEmailInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user != null && user.IsEmailConfirmed)
            {
                return;
            }

            if (user == null || user.EmailConfirmationCode.IsNullOrEmpty() || user.EmailConfirmationCode != input.ConfirmationCode)
            {
                throw new UserFriendlyException(L("InvalidEmailConfirmationCode"), L("InvalidEmailConfirmationCode_Detail"));
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationCode = null;

            await UserManager.UpdateAsync(user);
        }


        [AbpAuthorize(PermissionNames.Pages_Tenants_Impersonation_To_Tenant)]
        public virtual async Task<ImpersonateOutput> Impersonate(ImpersonateInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TenantId)
            };
        }
        [AbpAuthorize]
        public virtual async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNullAsync(AbpSession.ImpersonatorTenantId)
            };
        }

        //public virtual async Task<ImpersonateOutput> Impersonate(ImpersonateInput model)
        //{
        //    if (AbpSession.ImpersonatorUserId.HasValue)
        //    {
        //        throw new UserFriendlyException(L("CascadeImpersonationErrorMessage"));
        //    }

        //    if (AbpSession.TenantId.HasValue)
        //    {
        //        if (!model.TenantId.HasValue)
        //        {
        //            throw new UserFriendlyException(L("FromTenantToHostImpersonationErrorMessage"));
        //        }

        //        if (model.TenantId.Value != AbpSession.TenantId.Value)
        //        {
        //            throw new UserFriendlyException(L("DifferentTenantImpersonationErrorMessage"));
        //        }
        //    }

        //    return await SaveImpersonationTokenAndGetTargetUrl(model.TenantId, model.UserId, false);
        //}
        //public virtual async Task<ImpersonateOutput> BackToImpersonator()
        //{
        //    if (!AbpSession.ImpersonatorUserId.HasValue)
        //    {
        //        throw new UserFriendlyException(L("NotImpersonatedLoginErrorMessage"));
        //    }

        //    return await SaveImpersonationTokenAndGetTargetUrl(AbpSession.ImpersonatorTenantId, AbpSession.ImpersonatorUserId.Value, true);
        //}

        //private async Task<ImpersonateOutput> SaveImpersonationTokenAndGetTargetUrl(int? tenantId, long userId, bool isBackToImpersonator)
        //{
        //    //Create a cache item
        //    var cacheItem = new ImpersonationCacheItem(
        //        tenantId,
        //        userId,
        //        isBackToImpersonator
        //        );

        //    if (!isBackToImpersonator)
        //    {
        //        cacheItem.ImpersonatorTenantId = AbpSession.TenantId;
        //        cacheItem.ImpersonatorUserId = AbpSession.GetUserId();
        //    }

        //    //Create a random token and save to the cache
        //    var tokenId = Guid.NewGuid().ToString();
        //    await _cacheManager
        //        .GetImpersonationCache()
        //        .SetAsync(tokenId, cacheItem, TimeSpan.FromMinutes(1));

        //    //Find tenancy name
        //    string tenancyName = null;
        //    if (tenantId.HasValue)
        //    {
        //        tenancyName = (await _tenantManager.GetByIdAsync(tenantId.Value)).TenancyName;
        //    }
        //    return new ImpersonateOutput()
        //    {
        //        TenancyName = tenancyName,
        //        ImpersonationToken = tokenId
        //    };
           
        //}
        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }
        private async Task<User> GetUserByChecking(string inputEmailAddress)
        {
            var user = await UserManager.FindByEmailAsync(inputEmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }

            return user;
        }

        private async Task<string> GetTenancyNameOrNullAsync(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenantAsync(tenantId.Value)).TenancyName : null;
        }


        [NonAction]
        private string GenerateToken(int? id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                expires: DateTime.Now.AddYears(5),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
