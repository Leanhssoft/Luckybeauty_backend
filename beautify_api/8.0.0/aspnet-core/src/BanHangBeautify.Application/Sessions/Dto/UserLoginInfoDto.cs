using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BanHangBeautify.Authorization.Users;
using System;

namespace BanHangBeautify.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        public Guid? NhanSuId { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public Guid? IdChiNhanhMacDinh { get; set; }
    }
}
