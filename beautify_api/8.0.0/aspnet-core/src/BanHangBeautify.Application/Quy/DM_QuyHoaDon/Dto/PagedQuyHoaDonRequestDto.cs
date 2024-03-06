using System;
using System.Collections.Generic;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class PagedQuyHoaDonRequestDto : ParamSearch
    {
        public HashSet<int> HinhThucThanhToans { get; set; }
        public Guid? IdKhoanThuChi { get; set; }
        public Guid? IdTaiKhoanNganHang { get; set; }
    }
}
