using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using BanHangBeautify.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto : IShouldNormalize
    {
        [Required(ErrorMessage ="Tên tài khoản không được để trống")]
        [StringLength(AbpUserBase.MaxUserNameLength,ErrorMessage ="Tên tài khoản không được quá 256 ký tự")]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        //[Required]
        //[EmailAddress]
        //[StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public bool? IsAdmin { get; set; }

        public string[] RoleNames { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public Guid? NhanSuId { set; get; } = Guid.Empty;
        public Guid? IdChiNhanh { set; get; } = Guid.Empty;
        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }
}
