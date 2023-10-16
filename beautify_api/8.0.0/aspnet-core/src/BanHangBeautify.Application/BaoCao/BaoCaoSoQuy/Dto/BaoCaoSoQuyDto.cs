using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto
{
    public class BaoCaoSoQuyDto
    {
        public Guid Id { get; set; }
        public string MaHoaDon { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public decimal TienThu { get; set; }
        public decimal TienChi { get; set; }
        public decimal TonLuyKe { get; set; }
        public string NguoiNop { get; set; }
        public string GhiChu { get; set; }
    }
}
