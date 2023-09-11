using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Users.UserProfile
{
    [AbpAuthorize]
    public class UserProfileAppService: SPAAppServiceBase
    {
        private readonly UserManager _userManager;
        IRepository<NS_NhanVien, Guid> _nhanVienRepository;
        public UserProfileAppService(UserManager userManager, 
            IRepository<NS_NhanVien, Guid> nhanVienRepository)
        {
            _userManager = userManager;
            _nhanVienRepository = nhanVienRepository;
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
        [HttpPost]
        public async Task<bool> UpdateProfile(ProfileDto input)
        {
            var user = _userManager.GetUserById(input.Id);
            if (user == null)
            {
                return false;
            }
            user.Name = input.Name;
            user.Surname = input.Surname;
            user.PhoneNumber = input.PhoneNumber;
            user.EmailAddress = input.EmailAddress;
            if (user.NhanSuId != null)
            {
                var nhanSu = await _nhanVienRepository.FirstOrDefaultAsync(x => x.Id == input.NhanSuId);
                if (nhanSu != null)
                {
                    nhanSu.Avatar = input.Avatar;
                    nhanSu.SoDienThoai = input.PhoneNumber;
                    nhanSu.Ho = input.Name;
                    nhanSu.TenLot = input.Surname;
                    nhanSu.TenNhanVien = nhanSu.Ho + " " + nhanSu.TenLot;
                    nhanSu.CCCD = input.CCCD;
                    nhanSu.NgayCap = input.NgayCap;
                    if (!string.IsNullOrEmpty(input.NgaySinh))
                    {
                        nhanSu.NgaySinh = DateTime.Parse(input.NgaySinh);
                    }
                    await _nhanVienRepository.UpdateAsync(nhanSu);
                }
            }
            await _userManager.UpdateAsync(user);
            return true;
        }
       
        public async Task<ExecuteResultDto> ChangeUserPassword(ChangePasswordDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                result.Status = "error";
                result.Message = "Người dùng không tồn tại!";
            }
            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                var check = await _userManager.ChangePasswordAsync(user, input.NewPassword);
                if (check.Succeeded)
                {
                    result.Status = "success";
                    result.Message = "Thay đổi mật khẩu thành công!";
                }
                else
                {
                    result.Status = "error";
                    result.Message = string.Join(",", check.Errors.Select(x => x.Description).ToList());
                }
            }
            else
            {
                result.Status = "error";
                result.Message = "Mật khẩu hiện tại không đúng vui lòng kiểm tra lại";
            }

            return result;
        }
    }
}
