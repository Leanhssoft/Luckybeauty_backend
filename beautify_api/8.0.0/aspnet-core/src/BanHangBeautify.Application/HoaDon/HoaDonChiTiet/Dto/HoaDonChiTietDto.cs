using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.HoaDon.NhanVienThucHien.Dto;

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
        public double? SoLuong { get; set; } = 0;
        public double? DonGiaTruocCK { get; set; } = 0;
        public double? ThanhTienTruocCK { get; set; } = 0;
        public double? PTChietKhau { get; set; } = 0;
        public double? TienChietKhau { get; set; } = 0;
        public double? DonGiaSauCK { get; set; } = 0;
        public double? ThanhTienSauCK { get; set; } = 0;
        public double? PTThue { get; set; } = 0;
        public double? TienThue { get; set; } = 0;
        public double? DonGiaSauVAT { get; set; } = 0;
        public double? ThanhTienSauVAT { get; set; } = 0;
        public double? TonLuyKe { get; set; } = 0;
        public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1;// 0.Xóa, 1.Chưa xóa
        public List<NhanVienThucHienDto> nhanVienThucHien { get; set; }
    }
}
