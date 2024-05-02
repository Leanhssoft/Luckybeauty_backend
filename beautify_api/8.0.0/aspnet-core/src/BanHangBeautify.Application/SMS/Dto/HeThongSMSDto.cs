using BanHangBeautify.AppCommon;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;

namespace BanHangBeautify.SMS.Dto
{
    public class PageKhachHangSMSDto : CustomerBasicDto
    {
        public Guid? Id { get; set; }// sinhnhat (id = idkhachhang), lichhen (id = idbooking), hoadon (id= idhoadon) --> avoid error DataGrid MUI
        public DateTime? NgaySinh { set; get; } = null;
        public string MaHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { set; get; } = null;
        public double? TongThanhToan { set; get; } = 0;
        public double? DaThanhToan { set; get; } = 0;
        public string PTThanhToan { set; get; }
        public string TenHangHoa { get; set; }
        public string BookingCode { get; set; }
        public DateTime? BookingDate { set; get; } = null;
        public DateTime? StartTime { set; get; } = null;
        public int? ChenhLech { get; set; } // check gưi trước ? tiếng/phút/giây
        public string ThoiGianHen { set; get; } // từ HH:mm - HH:mm
        public string STrangThaiGuiTinNhan { get; set; }
        public int? TrangThai { get; set; }// used to bind background color

        public string ZOAUserId { get; set; }
        public Guid? IdHoaDon { get; set; }
        public Guid? IdBooking { get; set; }
        // đặt hẹn, giao dịch ở chi nhánh nào: used to background worker
        public Guid? IdChiNhanh { get; set; }
        public string TenChiNhanh { get; set; }
        public string SoDienThoaiChiNhanh { get; set; }
        public string DiaChiChiNhanh { get; set; } 
        public string TenCuaHang { get; set; }
        public string DienThoaiCuaHang { get; set; }
        public string DiaChiCuaHang { get; set; }
        public string XungHo { get; set; }// xưng hô khách hàng
    }

    public class CustomerWithZOA : CustomerBasicDto
    {
        public string ZOAUserId { get; set; }
        public Guid? IdHoaDon { get; set; }
        public Guid? IdBooking { get; set; }
    }

    public class ParamSearchSMS : CommonClass.ParamSearch
    {
        public List<byte> HinhThucGuiTins { get; set; }
        public bool? IsFilterCustomer { get; set; } = false;
        public byte? LoaiUser_CoTheGuiTin { get; set; } = 0;
    }
}
