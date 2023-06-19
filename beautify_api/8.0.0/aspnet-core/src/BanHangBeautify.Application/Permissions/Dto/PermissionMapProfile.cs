using Abp.Authorization;
using AutoMapper;
using BanHangBeautify.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Permissions.Dto
{
    public class PermissionMapProfile: Profile
    {
        public PermissionMapProfile()
        {
            CreateMap<Permission, FlatPermissionDto>().ReverseMap();
            CreateMap<Permission, FlatPermissionWithLevelDto>().ReverseMap();
            CreateMap<Permission, PermissionTreeDto>().ReverseMap();
        }
    }
}
