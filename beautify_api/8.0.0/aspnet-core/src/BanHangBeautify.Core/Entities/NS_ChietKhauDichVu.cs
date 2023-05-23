﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class NS_ChietKhauDichVu :FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid Id { get; set; }
        public Guid IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public Guid IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }

        public Guid IdDonViQuiDoi { get; set; }
        [ForeignKey("IdDonViQuiDoi")]
        public DM_DonViQuiDoi DM_DonViQuiDoi { get; set; }
        public byte LoaiChietKhau { get; set; }

        public decimal GiaTri { get; set; }
        public bool LaPhanTram { get; set; }
        public int TrangThai { get; set; }
    }
}