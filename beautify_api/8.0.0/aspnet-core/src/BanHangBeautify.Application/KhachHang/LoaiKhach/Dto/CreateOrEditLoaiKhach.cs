using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.KhachHang.LoaiKhach.Dto
{
    public class CreateOrEditLoaiKhachDto : EntityDto<int>
    {
        [MaxLength(10)]
        public string MaLoai { get; set; }
        [MaxLength(256)]
        public string TenLoai { get; set; }
        public int TrangThai { get; set; }
    }
}