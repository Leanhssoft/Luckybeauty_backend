using Abp.MultiTenancy;
using BanHangBeautify.Authorization.Users;
using System;

namespace BanHangBeautify.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }

        public bool IsTrial { get; set; }
        public DateTime SubscriptionEndDate { get; set; } 
    }
}
