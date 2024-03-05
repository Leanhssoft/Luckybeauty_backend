using BanHangBeautify.AppDashboard.Dto;
using System;

namespace BanHangBeautify.Suggests.Dto
{
    public class SuggestDichVuDto
    {
        public Guid Id { get; set; }
        public string TenDichVu { get; set; }
        public decimal DonGia { get; set; }
        public string ThoiGianThucHien { get; set; }
    }
}
