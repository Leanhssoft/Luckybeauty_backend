using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Data.Entities
{
    public class DM_HangHoa : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [MaxLength(256)]
        public string TenHangHoa { get; set; }
        [MaxLength(256)]
        public string TenHangHoa_KhongDau { get; set; }
        public int TrangThai { get; set; } = 1;
        public float? SoPhutThucHien { get; set; } = 0;
        public int IdLoaiHangHoa { get; set; }
        [ForeignKey("IdLoaiHangHoa")]
        public DM_LoaiHangHoa DM_LoaiHangHoa { get; set; }
        public Guid? IdNhomHangHoa { get; set; }
        [ForeignKey("IdNhomHangHoa")]
        public DM_NhomHangHoa DM_NhomHangHoa { get; set; }
        public int TenantId { get; set; }
        [MaxLength(2000)]
        public string MoTa { get; set; }
        public Guid? NguoiTao { get; set; }
        public Guid? NguoiSua { get; set; }
        public Guid? NguoiXoa { get; set; }
        public virtual ICollection<DM_DonViQuiDoi> DonViQuiDois { get; set; }
    }
}
