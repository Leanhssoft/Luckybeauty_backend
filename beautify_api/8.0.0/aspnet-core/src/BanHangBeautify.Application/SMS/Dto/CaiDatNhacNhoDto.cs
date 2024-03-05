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
        public Guid? IdMauTin { get; set; }
        public string NoiDungTin { get; set; }
        public float? NhacTruocKhoangThoiGian { get; set; } = 1;
        public byte? LoaiThoiGian { get; set; } = 1;
        public byte? TrangThai { get; set; } = 1;
        public List<CaiDatNhacNhoChiTietDto> CaiDatNhacNhoChiTiets { get; set; }
    }
    public class CaiDatNhacNhoChiTietDto
    {
        public Guid Id { get; set; }
        public Guid IdCaiDatNhacNho { get; set; }
        public byte? HinhThucGui { get; set; } = 0;
        public byte? TrangThai { get; set; } = 1;
    }
}
