﻿using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.HangHoa.DonViQuiDoi.Dto
{
    public class DonViQuiDoiDto : EntityDto<Guid>
    {
        public string MaHangHoa { get; set; }
        [MaxLength(50)]
        public string TenDonViTinh { get; set; }
        public float? TyLeChuyenDoi { get; set; } = 1;
        public double? GiaBan { get; set; } = 0;
        public double? GiaVon {  get; set; } = 0;
        public int? LaDonViTinhChuan { get; set; } = 1;
        public Guid IdHangHoa { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
