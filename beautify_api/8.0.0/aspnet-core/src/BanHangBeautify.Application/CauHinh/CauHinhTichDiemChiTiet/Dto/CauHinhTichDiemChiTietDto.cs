using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.CauHinh.CauHinhTichDiemChiTiet.Dto
{
    public class CauHinhTichDiemChiTietDto
    {
        public Guid Id{set;get;}
        public Guid IdTichDiem{set;get;}
        public Guid? IdNhomKhachHang{ set; get; }
    }
}
