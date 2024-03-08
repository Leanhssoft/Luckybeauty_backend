using System;
using System.Collections.Generic;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class PagedQuyHoaDonRequestDto : ParamSearch
    {
        public HashSet<int> HinhThucThanhToans { get; set; }
        public HashSet<byte> IdLoaiChungTus { get; set; } // 11.thu, 12.chi
        public int? IdLoaiChungTuLienQuan { get; set; } = 0;// 0.all, -1.khong lienquan hoadon, 1.hoadonban,..
        public Guid? IdKhoanThuChi { get; set; }
        public Guid? IdTaiKhoanNganHang { get; set; }
    }
}
