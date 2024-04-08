using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhacNho.NhacNhoHoatDong
{
    public interface INhacNhoHoatDongService: ITransientDependency
    {
        public Task SendEmailRemindActivity();

        public Task SendNotoficationContractExpired();
    }
}
