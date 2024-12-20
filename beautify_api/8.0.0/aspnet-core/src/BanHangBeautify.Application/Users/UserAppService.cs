﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
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
using BanHangBeautify.Consts;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Features;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using BanHangBeautify.Roles.Dto;
using BanHangBeautify.Users.Dto;
using BanHangBeautify.Users.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;
using User = BanHangBeautify.Authorization.Users.User;

namespace BanHangBeautify.Users
{
    [AbpAuthorize]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IRepository<NS_NhanVien, Guid> _nhanVienRepository;
        private readonly IUserRepository _userRepository;
        INhatKyThaoTacAppService _audilogService;
        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
        IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<NS_NhanVien, Guid> nhanVienRepository,
            IUserRepository userRepository,
            INhatKyThaoTacAppService audilogService
            )
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _nhanVienRepository = nhanVienRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _audilogService = audilogService;
        }

        [HttpGet]
        public async Task<bool> CheckMatchesPassword(long userId, string plainPassword)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            bool matchesPassword = await _userManager.CheckPasswordAsync(user, plainPassword);
            return matchesPassword;
        }

        [HttpGet]
        public async Task<bool> CheckExistUser(long userId, string userName)
        {
            if (userId == 0)
            {
                var lstUser = _userManager.Users.Where(x => x.UserName == userName).ToList();
                return lstUser.Count > 0;
            }
            else
            {
                var lstUser = _userManager.Users.Where(x => x.UserName == userName && x.Id != userId).ToList();
                return lstUser.Count > 0;
            }
        }

        [HttpGet]
        public async Task<bool> CheckExistEmail(long userId, string email)
        {
            if (userId == 0)
            {
                var lstUser = _userManager.Users.Where(x => x.EmailAddress == email).ToList();
                return lstUser.Count > 0;
            }
            else
            {
                var lstUser = _userManager.Users.Where(x => x.EmailAddress == email && x.Id != userId).ToList();
                return lstUser.Count > 0;
            }
        }

        [HttpPost]
        public async Task<UserDto> CreateUser(UpdateUserDto input)
        {
            var user = ObjectMapper.Map<User>(input);
            int maxUserCount = 0;
            var maxUser = FeatureChecker.GetValue(AbpSession.TenantId ?? 0, AppFeatureConst.MaxUserCount);
            if (maxUser != null && !string.IsNullOrEmpty(maxUser))
            {
                maxUserCount = int.Parse(maxUser);
            }
            var countUser = _userManager.Users.Count();
            if (maxUserCount > countUser || maxUserCount == 0)
            {
                CheckCreatePermission();
                user.TenantId = AbpSession.TenantId;
                user.NhanSuId = input.NhanSuId == Guid.Empty ? null : input.NhanSuId;
                user.IdChiNhanhMacDinh = input.IdChiNhanhMacDinh == Guid.Empty ? null : input.IdChiNhanhMacDinh;
                if (!string.IsNullOrEmpty(input.EmailAddress))
                {
                    user.IsEmailConfirmed = true;
                }
                else
                {
                    user.IsEmailConfirmed = false;
                }
                var nhanSu = _nhanVienRepository.FirstOrDefault(x => x.Id == input.NhanSuId);
                if (nhanSu != null)
                {
                    string[] tachChuoiTenNhanVien = nhanSu.TenNhanVien.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tachChuoiTenNhanVien.Length >= 2)
                    {
                        user.Name = string.Join(" ", tachChuoiTenNhanVien, 0, tachChuoiTenNhanVien.Length - 1);
                        user.Surname = tachChuoiTenNhanVien[tachChuoiTenNhanVien.Length - 1];
                    }
                    user.PhoneNumber = nhanSu.SoDienThoai;
                }
                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await _userManager.CreateAsync(user, input.Password));
                CurrentUnitOfWork.SaveChanges();
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
                nhatKyThaoTacDto.ChucNang = "Người dùng";
                nhatKyThaoTacDto.NoiDung = "Thêm mới người dùng: " + user.UserName;
                nhatKyThaoTacDto.NoiDungChiTiet = string.Format("<div>" +
                    "<p>Tên tài khoản: {0}</p>" +
                    "<p>Mật khẩu: {1}</p>" +
                    "<p>Email: {2}</p>" +
                    "</div>", user.UserName, input.Password, user.EmailAddress);
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return ObjectMapper.Map<UserDto>(user);
            }
            return null;
        }
        [HttpPost]
        public async Task<UserDto> UpdateUser_notRole(UpdateUserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);
            user.LastModificationTime = DateTime.Now;
            user.LastModifierUserId = AbpSession.UserId;
            user.IdChiNhanhMacDinh = input.IdChiNhanhMacDinh;
            user.NhanSuId = input.NhanSuId == Guid.Empty ? null : input.NhanSuId;
            user.IdChiNhanhMacDinh = input.IdChiNhanhMacDinh == Guid.Empty ? null : input.IdChiNhanhMacDinh;
            user.UserName = input.UserName;
            user.IsAdmin = input.IsAdmin ?? false;
            user.EmailAddress = input.EmailAddress;
            user.IsActive = input.IsActive;
            user.PhoneNumber = input.PhoneNumber;
            user.Name = input.Name;
            user.Surname = input.Surname;

            if (!string.IsNullOrEmpty(input.Password))
            {
                if (input.Password != user.Password)
                {
                    CheckErrors(await _userManager.ChangePasswordAsync(user, input.Password));
                }
            }

            CheckErrors(await _userManager.UpdateAsync(user));

            await _userManager.ChangeEmailAsync(user, input.EmailAddress, null);
            user.SetNormalizedNames();

            CurrentUnitOfWork.SaveChanges();

            return ObjectMapper.Map<UserDto>(user);
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
            var nhanSu = _nhanVienRepository.FirstOrDefault(x => x.Id == input.NhanSuId);
            if (nhanSu != null)
            {
                string[] tachChuoiTenNhanVien = nhanSu.TenNhanVien.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tachChuoiTenNhanVien.Length >= 2)
                {
                    user.Name = string.Join(" ", tachChuoiTenNhanVien, 0, tachChuoiTenNhanVien.Length - 1);
                    user.Surname = tachChuoiTenNhanVien[tachChuoiTenNhanVien.Length - 1];
                }
                user.PhoneNumber = nhanSu.SoDienThoai;
            }

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (userRole != null)
            {
                //userRole.IdChiNhanh = input.IdChiNhanh;
                _userRoleRepository.Update(userRole);
            }
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Người dùng";
            nhatKyThaoTacDto.NoiDung = "Thêm mới người dùng: " + user.UserName;
            nhatKyThaoTacDto.NoiDungChiTiet = string.Format("<div>" +
                "<p>Tên tài khoản: {0}</p>" +
                "<p>Mật khẩu: {1}</p>" +
                "<p>Email: {2}</p>" +
                "</div>", user.UserName, input.Password, user.EmailAddress);
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);


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
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Người dùng";
            nhatKyThaoTacDto.NoiDung = "Cập nhật thông tin người dùng: " + user.UserName;
            nhatKyThaoTacDto.NoiDungChiTiet = string.Format("<div>" +
                "<p>Tên tài khoản: {0}</p>" +
                "<p>Email: {1}</p>" +
                "</div>", user.UserName, user.EmailAddress) + string.Format("<br/><div>" +
                "<h4>Thông tin cũ</h4>" +
                "<p>Tên tài khoản: {0}</p>" +
                "<p>Email: {1}</p>" +
                "<p>Mật khẩu mới: {2}</p>" +
                "</div>", user.UserName, user.EmailAddress, input.Password);
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
            var nhanSu = _nhanVienRepository.FirstOrDefault(x => x.Id == input.NhanSuId);
            if (nhanSu != null)
            {
                string[] tachChuoiTenNhanVien = nhanSu.TenNhanVien.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tachChuoiTenNhanVien.Length >= 2)
                {
                    user.Name = string.Join(" ", tachChuoiTenNhanVien, 0, tachChuoiTenNhanVien.Length - 1);
                    user.Surname = tachChuoiTenNhanVien[tachChuoiTenNhanVien.Length - 1];
                }
                user.PhoneNumber = nhanSu.SoDienThoai;
            }
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
                var userRole = _userRoleRepository.GetAll().ToList();// (x => x.UserId == user.Id);
                //if (userRole != null)
                //{
                //    userRole.IdChiNhanh = input.IdChiNhanh;
                //    _userRoleRepository.Update(userRole);
                //}
            }
            var result = ObjectMapper.Map<UserDto>(user);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Người dùng";
            nhatKyThaoTacDto.NoiDung = "Cập nhật thông tin người dùng: " + user.UserName;
            nhatKyThaoTacDto.NoiDungChiTiet = string.Format("<div>" +
                "<p>Tên tài khoản: {0}</p>" +
                "<p>Email: {1}</p>" +
                "</div>", user.UserName, user.EmailAddress) + string.Format("<br/><div>" +
                "<h4>Thông tin cũ</h4>" +
                "<p>Tên tài khoản: {0}</p>" +
                "<p>Email: {1}</p>" +
                "<p>Mật khẩu mới: {2}</p>" +
                "</div>", user.UserName, user.EmailAddress, input.Password);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
            nhatKyThaoTacDto.ChucNang = "Người dùng";
            nhatKyThaoTacDto.NoiDung = "Xóa người dùng: " + user.UserName;
            nhatKyThaoTacDto.NoiDungChiTiet = "Xóa người dùng: " + user.UserName;
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
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Người dùng";
                nhatKyThaoTacDto.NoiDung = "Xóa người dùng: " + user.UserName;
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa người dùng: " + user.UserName;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<long> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
            };
            var checkExists = await Repository.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
            if (checkExists != null && checkExists.Count > 0)
            {
                Repository.RemoveRange(checkExists);
                result.Status = "success";
                result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Người dùng";
                nhatKyThaoTacDto.NoiDung = "Xóa người dùng: " + string.Join(", ", checkExists.Select(x => x.UserName).ToList());
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa người dùng: " + string.Join(", ", checkExists.Select(x => x.UserName).ToList());
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);

            }
            return result;
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Người dùng";
            nhatKyThaoTacDto.NoiDung = "Kích hoạt người dùng";

            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
                nhatKyThaoTacDto.NoiDungChiTiet = "Kích hoạt người dùng :" + entity.UserName;
            });

            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Người dùng";
            nhatKyThaoTacDto.NoiDung = "Hủy kích hoạt người dùng";
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
                nhatKyThaoTacDto.NoiDungChiTiet = "Hủy kích hoạt người dùng: " + entity.UserName;
            });

            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
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
            user.Password = "";
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.Password = "";
            userDto.ConfirmPassword = "";
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
        public async Task<PagedResultDto<UserProfileDto>> GetAllUser(ParamSearch input)
        {
            return await _userRepository.GetAllUser(input);
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
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Người dùng";
            nhatKyThaoTacDto.NoiDung = "Đặt lại mật khẩu";
            nhatKyThaoTacDto.NoiDungChiTiet = "Đặt lại mật khẩu cho người dùng: " + user.UserName;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return true;
        }

    }
}

