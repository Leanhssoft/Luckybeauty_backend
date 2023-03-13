using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BanHangBeautify.Entities;

namespace BanHangBeautify.Data.Entities
{
    public class DM_PhongBan : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        [MaxLength(50)]
        public string MaPhongBan { set; get; }
        [MaxLength(256)]
        public string TenPhongBan { set; get; }
        public Guid IdChiNhanh { set; get; }
        [ForeignKey("IdChiNhanh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }
        public DateTime NgayTao { set; get; }
        public Guid? NguoiTao { set; get; }
        public DateTime? NgaySua { set; get; }
        public Guid? NguoiSua { set; get; }
        public DateTime? NgayXoa { set; get; }
        public Guid? NguoiXoa { set; get; }
        public int TenantId { get; set; }
    }
}
