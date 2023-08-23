using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify
{
    public class BanHangBeautifyDomainServiceBase: DomainService
    {
        public BanHangBeautifyDomainServiceBase()
        {
            LocalizationSourceName = SPAConsts.LocalizationSourceName;
        }
    }
}
