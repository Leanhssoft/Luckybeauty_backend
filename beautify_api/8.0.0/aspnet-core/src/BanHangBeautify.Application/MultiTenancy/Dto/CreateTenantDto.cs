using Abp.AutoMapper;
using Abp.MultiTenancy;
using System;
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
        public string Password { get; set; }

        public bool IsDefaultPassword { set; get; }
        public bool IsActive { get; set; }
        [Required]
        public int EditionId { get; set; }

        public bool IsTrial { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
    }
}
