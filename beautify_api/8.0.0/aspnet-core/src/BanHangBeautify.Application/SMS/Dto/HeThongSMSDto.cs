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
        public string TenHangHoa { get; set; }
        public DateTime? BookingDate { set; get; } = null;
        public DateTime? StartTime { set; get; } = null;
        public string ThoiGianHen { set; get; } // từ HH:mm - HH:mm
        public string STrangThaiGuiTinNhan { get; set; }
        public int? TrangThai { get; set; }// used to bind background color
    }

    public class CustomerWithZOA : CustomerBasicDto
    {
        public string ZOAUserId { get; set; }
    }

    public class ParamSearchSMS : CommonClass.ParamSearch
    {
        public List<byte> HinhThucGuiTins { get; set; }
    }

}
