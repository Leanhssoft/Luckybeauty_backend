using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.SMS.GuiTinNhan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.GuiTinNhan
{
    public class NhatKyGuiTinSMSAppService : SPAAppServiceBase
    {
        private readonly IRepository<SMS_NhatKy_GuiTin, Guid> _nkyGuiTinSMS;
        public readonly IHeThongSMSRepository _repoSMS;

        public NhatKyGuiTinSMSAppService(IRepository<SMS_NhatKy_GuiTin, Guid> nkyGuiTinSMS, IHeThongSMSRepository repoSMS)
        {
            _nkyGuiTinSMS = nkyGuiTinSMS;
            _repoSMS = repoSMS;
        }
    }
}
