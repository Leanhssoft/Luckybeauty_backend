using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto
{
    public class CreateChiNhanhDto: EntityDto<Guid>
    {
        public int TenantId { get; set; }
        [Required]
        public Guid IdCongTy { set; get; }
        [MaxLength(50)]
        [Required]
        public string MaChiNhanh { get; set; }
        [MaxLength(2000)]
        [Required]
        public string TenChiNhanh { get; set; }
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
        public int TrangThai { get; set; }
        [Required]
        public DateTime NgayHetHan { get; set; }
        [Required]
        public DateTime NgayApDung { get; set; }


    }
}
