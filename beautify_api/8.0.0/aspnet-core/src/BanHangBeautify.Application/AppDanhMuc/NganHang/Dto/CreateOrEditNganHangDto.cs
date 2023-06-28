using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.NganHang.Dto
{
    public class CreateOrEditNganHangDto
    {
        public Guid Id { set; get; }
        public string MaNganHang { set; get; }
        public string TenNganHang { set; get; }
        public double ChiPhiThanhToan { set; get; }
        public bool TheoPhanTram { set; get; }
        public bool ThuPhiThanhToan { set; get; }
        public string ChungTuApDung { set; get; }
        public string GhiChu { set; get; }
        public int TrangThai { set; get; }
    }
}
