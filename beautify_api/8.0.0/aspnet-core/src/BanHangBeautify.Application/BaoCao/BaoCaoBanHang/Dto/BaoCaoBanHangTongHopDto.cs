using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoBanHang.Dto
{
    public class BaoCaoBanHangTongHopDto
    {
        public string TenHangHoa { set; get; }
        public string MaHangHoa { get; set; }
        public string NhomHangHoa { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public decimal DoanhThu { get; set; }
    }
}
