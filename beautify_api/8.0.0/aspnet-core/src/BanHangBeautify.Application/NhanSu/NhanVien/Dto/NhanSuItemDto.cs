using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien.Dto
{
    public class NhanSuItemDto : EntityDto<Guid>
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string CCCD { get; set; }
        public DateTime NgaySinh { get; set; }
        public int KieuNgaySinh { get; set; }
        public int GioiTinh { get; set; }
        [MaxLength(256)]
        public string NgayCap { get; set; }
        [MaxLength(2000)]
        public string NoiCap { get; set; }
        public DateTime NgayVaoLam { set; get; }
        public byte[] Avatar { get; set; }
        public string TenChucVu { set; get; }
    }
}
