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
        public Guid? IdChiNhanh { get; set; }
        public string StartTime { get; set; }
        public string StartHours { get; set; }
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
        public Guid IdKhachHang { get; set; }
        public Guid IdNhanVien { get; set; }
        public Guid IdDonViQuiDoi { get; set; }
    }
}
