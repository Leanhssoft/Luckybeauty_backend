using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.NhanSu.CaLamViec.Dto
{
    public class CreateOrEditCaLamViecDto : EntityDto<Guid>
    {
        [MaxLength(50)]
        public string MaCa { set; get; }
        [MaxLength(256)]
        public string TenCa { set; get; }
        public string GioVao { set; get; }
        public string GioRa { set; get; }
        public bool LaNghiGiuaCa { set; get; }
        public string GioNghiTu { get; set; }
        public string GioNghiDen { get; set; }
        public float TongGioCong { set; get; }
        public Guid IdChiNhanh { get; set; }
    }
}
