using Abp.Authorization.Users;
using Abp.AutoMapper;
using BanHangBeautify.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class UpdateUserDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Tên tài khoản không được để trống")]
        [StringLength(AbpUserBase.MaxUserNameLength,ErrorMessage ="Tên tài khoản không được quá 256 ký tự")]
        public string UserName { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; } 
        //[Phone]
        public string PhoneNumber { get; set; }
        //[Required]
        //[EmailAddress]
        //[StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        public string Password { set; get; }
        public bool IsActive { get; set; }
        public bool? IsAdmin { get; set; }

        public string[] RoleNames { get; set; }
        public Guid? NhanSuId { set; get; } = null;
        public Guid? IdChiNhanhMacDinh { set; get; } = null;
    }
}
