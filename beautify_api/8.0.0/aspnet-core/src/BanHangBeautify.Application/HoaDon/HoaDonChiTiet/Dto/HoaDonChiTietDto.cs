using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDonChiTiet.Dto
{
    public class HoaDonChiTietDto
    {
        public int TenantId { get; set; } = 1;
        public Guid Id { get; set; }
        public Guid IdHoaDon { get; set; }
        public BH_HoaDon BH_HoaDon { get; set; }
        public int STT { get; set; } = 1;
        public Guid IdDonViQuyDoi { get; set; }
        public Guid? IdChiTietHoaDon { get; set; }// sử dụng khi trả hàng, lưu chi tiết hóa đơn gốc ban đầu
        public float? SoLuong { get; set; } = 0;
        public float? DonGiaTruocCK { get; set; } = 0;
        public float? ThanhTienTruocCK { get; set; } = 0;
        public float? PTChietKhau { get; set; } = 0;
        public float? TienChietKhau { get; set; } = 0;
        public float? DonGiaSauCK { get; set; } = 0;
        public float? ThanhTienSauCK { get; set; } = 0;
        public float? PTThue { get; set; } = 0;
        public float? TienThue { get; set; } = 0;
        public float? DonGiaSauVAT { get; set; } = 0;
        public float? ThanhTienSauVAT { get; set; } = 0;
        public float? TonLuyKe { get; set; } = 0;
        public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1;// 0.Xóa, 1.Chưa xóa
    }
}
