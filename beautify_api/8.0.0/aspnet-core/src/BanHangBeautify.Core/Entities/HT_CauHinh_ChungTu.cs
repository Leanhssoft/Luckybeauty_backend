using Abp.Domain.Entities;
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
    public class HT_CauHinh_ChungTu : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdChiNhanh{set;get;}
        [ForeignKey(nameof(IdChiNhanh))]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public int IdLoaiChungTu{set;get;}
        [ForeignKey(nameof(IdLoaiChungTu))]
        public DM_LoaiChungTu DM_LoaiChungTu { get; set; }
        public string MaLoaiChungTu{set;get;}
        public bool SuDungMaChiNhanh{set;get;}
        [MaxLength(2)]
        public string KiTuNganCach1{set;get;}
        [MaxLength(2)]
        public string KiTuNganCach2{set;get;}
        [MaxLength(15)]
        public string NgayThangNam{set;get;}
        [MaxLength(2)]
        public string KiTuNganCach3{set;get;}
        public int DoDaiSTT{set;get;}
    }
}
