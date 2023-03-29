using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.Dto
{
    public class CreateBookingDto
    {
        public int TenantId { get; set; }
        [Required]
        public string TenKhachHang { get; set; }
        [Required]
        public string SoDienThoai { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public byte LoaiBooking { get; set; }
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
    }
}
