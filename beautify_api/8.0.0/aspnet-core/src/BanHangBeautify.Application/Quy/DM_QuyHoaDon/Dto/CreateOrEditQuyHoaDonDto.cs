using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class CreateOrEditQuyHoaDonDto
    {
        public Guid Id{set;get;}
        public Guid IdChiNhanh{set;get;}
        public int IdLoaiChungTu{set;get;}
        public string MaHoaDon{set;get;}
        public DateTime NgayLapHoaDon{set;get;}
        public float? TongTienThu{set;get;}
        public string NoiDungThu{set;get;}
        public bool HachToanKinhDoanh { set; get; }
    }
}
