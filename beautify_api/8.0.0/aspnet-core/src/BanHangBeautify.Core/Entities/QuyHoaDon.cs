﻿using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Entities
{
    public class QuyHoaDon : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [Required]
        public int TenantId { get; set; } = 1;
        public int IdLoaiChungTu { get; set; } = 11;
        [ForeignKey("IdLoaiChungTu")]
        public DM_LoaiChungTu DM_LoaiChungTu { get; set; }
        [Required, MaxLength(256)]
        public string MaHoaDon { get; set; } = string.Empty;
        [Required]
        public DateTime NgayLapHoaDon { get; set; } = DateTime.Now;
        public Guid? IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public float? TongTienThu { get; set; } = 0;
        [MaxLength(4000)]
        public string NoiDungThu { get; set; } = string.Empty;
        public bool? HachToanKinhDoanh { get; set; } = true;
        public int TrangThai { get; set; } = 1;//0.xoa, 1.chua xoa
    }
}
