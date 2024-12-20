using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantDto : EntityDto
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }
        public string ConnectionString { set; get; }
        [Required]
        public int? EditionId { get; set; }
        public bool IsActive { get; set; }

        public bool IsTrial { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
    }
}
