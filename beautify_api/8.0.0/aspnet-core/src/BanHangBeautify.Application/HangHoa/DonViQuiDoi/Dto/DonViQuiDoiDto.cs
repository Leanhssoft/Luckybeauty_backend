﻿using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.HangHoa.DonViQuiDoi.Dto
{
    public class DonViQuiDoiDto : EntityDto<Guid>
    {
        public string MaHangHoa { get; set; }
        [MaxLength(50)]
        public string TenDonVi { get; set; }
        public decimal TyLeChuyenDoi { get; set; }
        public decimal GiaBan { get; set; }
        public int LaDonViTinhChuan { get; set; }
        public Guid IdHangHoa { get; set; }
    }
}
