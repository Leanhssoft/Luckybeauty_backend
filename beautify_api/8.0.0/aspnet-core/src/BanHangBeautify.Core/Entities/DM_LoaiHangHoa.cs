﻿using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Data.Entities
{
    public class DM_LoaiHangHoa : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public string  MaLoai { get; set; }
        public string TenLoai{ get; set; }
        public int TenantId { get ; set ; }
        public int TrangThai { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public Guid? NguoiXoa { get; set; }
        public DateTime? NgayXoa { get; set; }
    }
}
