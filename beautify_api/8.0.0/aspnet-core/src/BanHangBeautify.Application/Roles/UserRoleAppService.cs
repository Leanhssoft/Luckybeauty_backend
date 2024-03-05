using Abp.Application.Editions;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserRoleAppService(
           IRepository<UserRole, long> userRole, IUnitOfWorkManager unitOfWorkManager
       )
        {
            _userRole = userRole;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [HttpPost]
        public async Task CreateUserRoleAsync(CreateOrUpdateUserRoleDto input)
        {
            var userRoleNew = new UserRoleChiNhanh(input.TenantId, input.UserId, input.RoleId, input.IdChiNhanh);
            await _userRole.InsertAsync(userRoleNew);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
        }


        [HttpPost]
        public async Task CreateRole_byChiNhanhOfUser(long userId, List<CreateOrUpdateUserRoleDto> lst)
        {
            if (lst != null)
            {
                // get role old && remove
                var arrRoleOld = _userRole.GetAll().Where(x => x.UserId == userId).ToList();
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete)) // xoa vinh vien
                {
                    foreach (var item in arrRoleOld)
                    {
                        await _userRole.DeleteAsync(item.Id);
                    }
                }

                // add again
                var tenantId = AbpSession.TenantId;
                foreach (var item in lst)
                {
                    var userRoleNew = new UserRoleChiNhanh(item.TenantId?? tenantId, userId, item.RoleId, item.IdChiNhanh);
                    await _userRole.InsertAsync(userRoleNew);
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
        }

        public async Task<List<UserRole>> GetRolebyChiNhanh_ofUser(long userId)
        {
            var data = _userRole.GetAll().Where(x=>x.UserId==userId).ToList();
            return data;
        }
    }
}
