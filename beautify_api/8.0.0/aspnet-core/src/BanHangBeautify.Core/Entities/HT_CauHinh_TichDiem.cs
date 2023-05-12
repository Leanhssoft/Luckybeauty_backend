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
        public HT_CauHinhPhanMem HT_CauHinhPhanMem { get; set; }
        public decimal TyLeDoiDiem{set;get;}
        public bool ChoPhepThanhToanBangDiem{set;get;}
        public decimal DiemThanhToan{set;get;}
        public decimal TienThanhToan{set;get;}
        public bool KhongTichDiemHDGiamGia{set;get;}
        public bool TichDiemHoaDonGiamGia{set;get;}
        public bool KhongTichDiemSPGiamGia{set;get;}
        public bool TatCaKhachHang{set;get;}
        public int SoLanMua{set;get;}
    }
}
