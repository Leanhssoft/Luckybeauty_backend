using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.SMS.Dto
{
    public class CreateOrEditHeThongSMSDto
    {
        public Guid Id { set; get; }
        public Guid IdChiNhanh { set; get; }
        public Guid IdNguoiGui { set; get; }
        public Guid IdKhachHang { set; get; }
        public Guid IdHoaDon { set; get; }
        public Guid IdTinNhan { set; get; }
        public string SoDienThoai { set; get; }
        public int SoTinGui { set; get; }
        public string NoiDungTin { set; get; }
        public DateTime ThoiGianGui { set; get; }
        public byte LoaiTin { set; get; }
        public double PhiGuiTin { get; set; }
        public int TrangThai { get; set; }
    }
}
