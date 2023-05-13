﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class NS_NhanVien_TimeOff:FullAuditedEntity<Guid>,IMustHaveTenant
    {
        public int TenantId { set; get; }
        public Guid IdNhanVien { get; set; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien { get; set; }
        public DateTime TuNgay { set; get; }
        public DateTime DenNgay { set; get; }
        public int LoaiNghi { get; set; }
        public double TongNgayNghi { set; get; }
    }
}
