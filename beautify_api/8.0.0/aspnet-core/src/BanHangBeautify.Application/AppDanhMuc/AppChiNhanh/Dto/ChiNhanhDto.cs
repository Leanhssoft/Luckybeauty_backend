using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto
{
    public class ChiNhanhDto
    {
        [Required]
        public Guid IdCongTy { get; set; }
        public string MaChiNhanh { get; set; }
        public string TenChiNhanh { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
        public string Logo { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayHetHan { get; set; }
        public DateTime NgayApDung { get; set; }
    }
}
