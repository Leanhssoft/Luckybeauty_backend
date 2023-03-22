using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.PhongBan.Dto
{
    public class CreateOrEditPhongBanDto : EntityDto<Guid>
    {
        [MaxLength(50)]

        public string MaPhongBan { set; get; }
        [Required]
        [MaxLength(256)]
        public string TenPhongBan { set; get; }
        [Required]
        public Guid IdChiNhanh { set; get; }
    }
}
