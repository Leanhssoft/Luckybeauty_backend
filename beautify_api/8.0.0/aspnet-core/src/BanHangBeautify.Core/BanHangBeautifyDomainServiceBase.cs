using Abp.Domain.Services;

namespace BanHangBeautify
{
    public class BanHangBeautifyDomainServiceBase : DomainService
    {
        public BanHangBeautifyDomainServiceBase()
        {
            LocalizationSourceName = SPAConsts.LocalizationSourceName;
        }
    }
}
