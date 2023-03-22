using Abp.MultiTenancy;
using BanHangBeautify.Authorization.Users;

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
    }
}
