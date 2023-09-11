using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Roles.Dto;
using BanHangBeautify.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Users
{
    [AbpAuthorize(PermissionNames.Pages_Administration_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IRepository<NS_NhanVien, Guid> _nhanVienRepository;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IRepository<NS_NhanVien, Guid> nhanVienRepository)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _nhanVienRepository = nhanVienRepository;
        }

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);
            user.IsAdmin = input.IsAdmin ?? false;
            user.TenantId = AbpSession.TenantId;
            if (!string.IsNullOrEmpty(input.EmailAddress))
            {
                user.IsEmailConfirmed = true;
            }
            else
            {
                user.IsEmailConfirmed = false;
            }


            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }
        [HttpPost]
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);
            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }
        [HttpPost]
        public async Task<UserDto> UpdateUser(UpdateUserDto input)
            {
            CheckUpdatePermission();
            var user = await _userManager.FindByIdAsync(input.Id.ToString());
            //var user = await _userManager.GetUserByIdAsync(input.Id);
            user.NhanSuId = input.NhanSuId;
            user.IsAdmin = input.IsAdmin ?? false;
            user.Surname = input.Surname;
            user.Name = input.Name;
            user.PhoneNumber = input.PhoneNumber;
            user.EmailAddress = input.EmailAddress;
            user.IsActive = input.IsActive;
            user.LastModificationTime = DateTime.Now;
            user.LastModifierUserId = AbpSession.UserId;
            
            if (!string.IsNullOrEmpty(input.Password))
            {
                if (input.Password != user.Password)
                {
                    CheckErrors(await _userManager.ChangePasswordAsync(user, input.Password));
                }
            }
            
            await _userManager.ChangeEmailAsync(user, input.EmailAddress, null);
            user.SetNormalizedNames();

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }
            var result = ObjectMapper.Map<UserDto>(user);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task<bool> DeleteUser(EntityDto<long> input)
        {
            bool result = false;
            try
            {
                var user = await _userManager.GetUserByIdAsync(input.Id);
                user.IsActive = false;
                user.IsDeleted = true;
                user.DeleterUserId = AbpSession.UserId;
                user.DeletionTime = DateTime.Now;
                await _userManager.UpdateAsync(user);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }


        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.Password = user.Password;
            userDto.ConfirmPassword = userDto.Password;
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        public async Task<ProfileDto> GetForUpdateProfile()
        {
            var user = _userManager.GetUserById(AbpSession.UserId ?? 0);

            if (user != null)
            {
                var result = new ProfileDto();
                result.Id = user.Id;
                result.NhanSuId = user.NhanSuId;
                result.Surname = user.Surname;
                result.Name = user.Name;
                result.UserName = user.UserName;
                result.PhoneNumber = user.PhoneNumber;
                result.EmailAddress = user.EmailAddress;
                var nhanSu = await _nhanVienRepository.FirstOrDefaultAsync(x => x.Id == user.NhanSuId);
                if (nhanSu != null)
                {
                    result.Avatar = nhanSu.Avatar;
                    result.CCCD = nhanSu.CCCD;
                    result.NgayCap = nhanSu.NgayCap;
                    result.NoiCap = nhanSu.NoiCap;
                    result.GioiTinh = nhanSu.GioiTinh;
                    result.DiaChi = nhanSu.DiaChi;
                }
                return result;
            }
            return new ProfileDto();
        }
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }

            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }

            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }
}

