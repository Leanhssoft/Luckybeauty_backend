﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class DM_KhuyenMai_ApDung : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { set; get; }
        public Guid IdKhuyenMai { set; get; }
        [ForeignKey(nameof(IdKhuyenMai))]

        public DM_KhuyenMai DM_KhuyenMai { get; set; }
        public Guid? IdChiNhanh { set; get; }
        public Guid? IdNhomKhach { set; get; }
        public Guid? IdNhanVien { set; get; }
    }
}