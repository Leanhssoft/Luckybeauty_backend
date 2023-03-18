using Abp.Domain.Entities;
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
    public class NS_LichLamViec : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { set; get; }
        public Guid IdChiNhanh { set; get; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { set; get; }
        public Guid IdNhanVien { set; get; }
        [ForeignKey("IdNhanVien")]
        public NS_NhanVien NS_NhanVien{set;get ;}
        public DateTime TuNgay    {set;get;}
        public DateTime? DenNgay   {set;get;}
        [MaxLength(2000)]
        public string GhiChu    {set;get;}
        public int TrangThai {set;get;}
        public bool LapLai    {set;get;}
        public int KieuLapLai{set;get;}
        public int GiaTriLap { get; set; }
    }
}
