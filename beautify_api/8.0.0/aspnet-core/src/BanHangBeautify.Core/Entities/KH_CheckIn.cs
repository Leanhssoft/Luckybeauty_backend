﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class KH_CheckIn : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; } = 1;
        public Guid? IdChiNhanh { get; set; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public Guid IdKhachHang { get; set; }
        [ForeignKey("IdKhachHang")]
        public DM_KhachHang DM_KhachHang { get; set; }
        public DateTime DateTimeCheckIn { get; set; }// ngay check in yyyy-mm-dd hh:mm:ss
        [MaxLength(4000)]
        public string GhiChu { get; set; } = string.Empty;
        public int TrangThai { get; set; } = 1; // 0. Xóa khách checkin (do thêm sai), 1.dang check in & đang làm dịch vụ (mặc định), 2. đã check in & làm xong dịch vụ, 3.Khách đến check in nhưng đợi lâu quá (Cancel)
    }
}
