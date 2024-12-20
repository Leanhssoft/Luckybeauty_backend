﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class HT_CauHinh_TichDiemChiTiet : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdTichDiem { get; set; }
        [ForeignKey(nameof(IdTichDiem))]
        public HT_CauHinh_TichDiem HT_CauHinh_TichDiem { get; set; }
        public Guid? IdNhomKhachHang { get; set; }
    }
}
