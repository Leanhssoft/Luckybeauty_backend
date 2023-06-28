using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class HT_CauHinh_TichDiem : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public Guid IdCauHinh{set;get;}
        public float? TyLeDoiDiem { set; get; } = 0;
        public bool? ChoPhepThanhToanBangDiem { set; get; } = false;
        public float? DiemThanhToan { set; get; } = 0;
        public float? TienThanhToan { set; get; } = 0;
        public bool? KhongTichDiemHDGiamGia{set;get;} = false;
        public bool? TichDiemHoaDonGiamGia{set;get;} = false;
        public bool? KhongTichDiemSPGiamGia { set; get; } = false;
        public bool? TatCaKhachHang{set;get;}
        public int? SoLanMua { set; get; } = 0;
    }
}
