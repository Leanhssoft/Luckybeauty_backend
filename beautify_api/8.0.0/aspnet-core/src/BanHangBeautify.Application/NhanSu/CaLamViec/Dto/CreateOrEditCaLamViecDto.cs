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
        public DateTime GioVao { set; get; }
        public DateTime GioRa { set; get; }
        public float TongGioCong { set; get; }
    }
}
