﻿using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.SMS.ESMS
{
    public interface IESMS
    {
        Task<ResultSMSDto> SendSMS_Json(ESMSDto obj);
    }
}
