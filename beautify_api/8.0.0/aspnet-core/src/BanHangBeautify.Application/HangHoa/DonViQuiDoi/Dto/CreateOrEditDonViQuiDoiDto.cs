using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.HangHoa.DonViQuiDoi.Dto
{
    public class CreateOrEditDonViQuiDoiDto : EntityDto<Guid>
    {
        public string MaHangHoa { get; set; }
        [MaxLength(50)]
        public string TenDonViTinh { get; set; }
        public float? TyLeChuyenDoi { get; set; } = 1;
        public float? GiaBan { get; set; } = 0;
        public int? LaDonViTinhChuan { get; set; } = 1;
        public Guid IdHangHoa { get; set; }
    }
}
