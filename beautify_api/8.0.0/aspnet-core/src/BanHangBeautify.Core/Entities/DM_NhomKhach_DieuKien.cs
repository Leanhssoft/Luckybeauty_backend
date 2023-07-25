using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class DM_NhomKhach_DieuKien : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public byte STT { get; set; }
        public Guid IdNhomKhach { get; set; }
        [ForeignKey("IdNhomKhach")]
        public DM_NhomKhachHang DM_NhomKhachHang { get; set; }

        public byte LoaiDieuKien { get; set; }
        public byte LoaiSoSanh { get; set; } // 1: > , 2: >= , 3: = , 4: <=, 5: <, 6 : khác

        public float GiaTriSo { get; set; }
        public bool GiaTriBool { get; set; }
        public DateTime? GiaTriThoiGian { get; set; }
        public Guid? GiaTriKhuVuc { get; set; }
    }
}
