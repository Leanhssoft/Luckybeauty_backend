using Abp.AutoMapper;
using Abp.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.MultiTenancy.Dto
{
    [AutoMapTo(typeof(Tenant))]
    public class CreateTenantDto
    {
        [Required]
        [StringLength(16)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }
        [Required(ErrorMessage = "Please enter your Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length between 6 than 50 characters.")]
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
