using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Dto
{
    public class CreateOrEditTaiKhoanNganHangDto
    {
        public Guid Id{set;get;}
        public Guid? IdChiNhanh{set;get;}
        public Guid IdNganHang{set;get;}
        public string SoTaiKhoan{set;get;}
        public string TenChuThe{set;get;}
        public string GhiChu{set;get;}
        public int TrangThai { set; get; } = 1;
    }
}
