﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class HT_SMSBrandname : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [MaxLength(100)]
        public string Brandname { get; set; }
        [MaxLength(100)]
        public string SDTCuaHang { get; set; }
        public byte? TrangThai { get; set; } = 1; // = tinyint sql (0-255), 1. Kích hoạt, 0.chưa kích hoạt
        public DateTime? NgayKichHoat { get; set; } = DateTime.Now;
    }
}
