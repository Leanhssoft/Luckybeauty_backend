using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.MauTinSMS
{
    public interface IMauTinSMSRepository
    {
        Task<MauTinSMSDto> GetMauTinSMS_byId(Guid id);
    }
}
