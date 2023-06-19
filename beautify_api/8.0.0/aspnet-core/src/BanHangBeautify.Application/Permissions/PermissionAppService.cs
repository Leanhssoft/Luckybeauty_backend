using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Permissions.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Permissions
{
    public class PermissionAppService : SPAAppServiceBase, IPermissionAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        public PermissionAppService(IRepository<User, long> userRepository, IRepository<Role> roleRepositor, IRepository<UserRole, long> userRoleRepository)
        {
            //_roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepositor;
        }
        [HttpPost]
        public async Task<GetPermissionDto> GetAllPermissionByRole(long UserId)
        {

            GetPermissionDto data = new GetPermissionDto();
            try
            {
                var userIdSession = AbpSession.UserId;
                List<string> listPermissison = new List<string>();
                var user = await _userRepository.GetAsync(UserId);
                if (user != null)
                {
                    data.Name = user.Name;
                    var roles = _userRepository.GetAllIncluding(x => x.Roles).Where(x => x.Id == UserId).Select(x => x.Roles).ToList();
                    var roleIds = roles[0].Select(x => x.RoleId).ToList();
                    var rolePermissions = _roleRepository.GetAllIncluding(x => x.Permissions).Where(x => roleIds.Contains(x.Id)).ToList();
                    if (rolePermissions != null && rolePermissions.Count > 0)
                    {
                        foreach (var item in rolePermissions)
                        {
                            var permissions = item.Permissions.Where(x=>x.IsGranted==true).Select(x => x.Name).ToList();
                            foreach (var permission in permissions)
                            {
                                listPermissison.Add(permission);
                            }
                        }
                    }
                    data.Permissions = listPermissison.Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                data = null;
            }
            return data;
        }
        public ListResultDto<PermissionTreeDto> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();
            var rootPermissions = permissions.Where(p => p.Parent == null).ToList();
            permissions = permissions.Where(x => x.Name=="Pages").ToList();
            var result = new List<PermissionTreeDto>();
            foreach (var rootPermission in rootPermissions)
            {
                AddPermission(rootPermission, permissions, result);
            }

            return new ListResultDto<PermissionTreeDto>
            {
                Items = result
            };
        }
        private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<PermissionTreeDto> result)
        {
            var flatPermission = ObjectMapper.Map<PermissionTreeDto>(permission);
            if (AbpSession.MultiTenancySide!=Abp.MultiTenancy.MultiTenancySides.Host)
            {
                flatPermission.Children = flatPermission.Children.Where(x => x.Name != "Pages.Tenants").ToList();
            }
            result.Add(flatPermission);
            if (permission.Children == null)
            {
                return;
            }

            var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

            foreach (var childPermission in children)
            {
                AddPermission(childPermission, allPermissions, result);
            }
        }
    }

}
