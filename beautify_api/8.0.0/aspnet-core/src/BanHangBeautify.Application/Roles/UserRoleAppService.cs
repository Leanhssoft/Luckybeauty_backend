using Abp.Application.Editions;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Editions;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDon.Exporting;
using BanHangBeautify.HoaDon.HoaDon.Repository;
using BanHangBeautify.HoaDon.NhanVienThucHien;
using BanHangBeautify.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Roles
{
    public class UserRoleAppService : SPAAppServiceBase
    {
        private readonly IRepository<UserRole, long> _userRole;
        public UserRoleAppService(
           IRepository<UserRole, long> userRole
       )
        {
            _userRole = userRole;
        }

        [HttpPost]
        public async Task CreateUserRoleAsync(CreateOrUpdateUserRoleDto input)
        {
            var userRoleNew = new UserRoleChiNhanh(input.TenantId, input.UserId, input.RoleId, input.IdChiNhanh);
            await _userRole.InsertAsync(userRoleNew);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
        }
        public async Task UpdateUserRoleAsync(CreateOrUpdateUserRoleDto input)
        {
            var userRole = (UserRoleChiNhanh)await _userRole.FirstOrDefaultAsync(input.Id);
            if (userRole != null)
            {
                userRole.UserId = input.UserId;
                userRole.RoleId = input.RoleId;
                userRole.IdChiNhanh = input.IdChiNhanh;
                await _userRole.UpdateAsync(userRole);
                await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
            }
        }
    }
}
