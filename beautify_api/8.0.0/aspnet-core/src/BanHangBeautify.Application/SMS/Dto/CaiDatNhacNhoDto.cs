using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Dto
{
    public class CaiDatNhacNhoDto
    {
        public Guid Id { get; set; }
        public int TenantId { get; set; }
        public byte IdLoaiTin { get; set; }
        public float? NhacTruocKhoangThoiGian { get; set; } = 1;
        public byte? LoaiThoiGian { get; set; } = 1;
        public byte? TrangThai { get; set; } = 1;
        public byte? HinhThucGui { get; set; } = 0;
        public string IdMauTin { get; set; }
    }

    public class CaiDatNhacNho_GroupLoaiTinDto
    {
        public byte IdLoaiTin { get; set; }
        public List<CaiDatNhacNhoDto> LstDetail { get; set; }
    }
}
