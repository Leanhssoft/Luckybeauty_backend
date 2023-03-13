using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppCuaHang.Dto
{
    public class CreateOrEditCuaHangDto: EntityDto<Guid>
    {
        [Required]
        [MaxLength(2000)]
        public string TenCongTy { get; set; }
        [MaxLength(256)]
        [Required]
        public string SoDienThoai { get; set; }

        [MaxLength(2000)]
        public string DiaChi { get; set; }
        [MaxLength(256)]
        [Required]
        public string MaSoThue { get; set; }
        [MaxLength(2000)]
        public string Logo { get; set; }
        [MaxLength(2000)]
        public string GhiChu { get; set; }

        public string? MaChiNhanh { get; set; }
        
        public string? TenChiNhanh { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public DateTime? NgayApDung { get; set; }
    }
}
