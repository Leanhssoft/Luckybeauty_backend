using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using BanHangBeautify.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required(AllowEmptyStrings = true)]
        //[EmailAddress]
        //[StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }
        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }

        public Guid? NhanSuId { get; set; }
        public Guid? IdChiNhanhMacDinh { get; set; }
    }
}
