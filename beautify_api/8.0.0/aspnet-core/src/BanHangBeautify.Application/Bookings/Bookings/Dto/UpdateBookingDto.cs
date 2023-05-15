using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class UpdateBookingDto: Entity<Guid>
    {
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public byte LoaiBooking { get; set; }
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayXuLy { get; set; }
        public string XuLy_GhiChu { get; set; }
        public Guid? IdKhachHang { get; set; }
        public Guid? IdChiNhanh { get; set; }
        public Guid IdNhanVien { get; set; }
        public Guid IdDonViQuiDoi { get; set; }
    }
}
