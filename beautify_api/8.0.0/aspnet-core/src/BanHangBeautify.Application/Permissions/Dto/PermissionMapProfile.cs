using Abp.Authorization;
using AutoMapper;
using BanHangBeautify.Roles.Dto;

namespace BanHangBeautify.Permissions.Dto
{
    public class PermissionMapProfile : Profile
    {
        public PermissionMapProfile()
        {
            CreateMap<Permission, FlatPermissionDto>().ReverseMap();
            CreateMap<Permission, FlatPermissionWithLevelDto>().ReverseMap();
            CreateMap<Permission, PermissionTreeDto>().ReverseMap();
        }
    }
}
