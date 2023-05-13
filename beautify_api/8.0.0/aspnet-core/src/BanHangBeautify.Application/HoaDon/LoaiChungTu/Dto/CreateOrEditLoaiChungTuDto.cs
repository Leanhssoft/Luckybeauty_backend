using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.ChungTu.Dto
{
    public class CreateOrEditLoaiChungTuDto
    {
        public int Id { get; set; }
        public string MaLoaiChungTu { get; set; }
        public string TenLoaiChungTu { get; set; }
        public int TrangThai { get; set; }
    }
}
