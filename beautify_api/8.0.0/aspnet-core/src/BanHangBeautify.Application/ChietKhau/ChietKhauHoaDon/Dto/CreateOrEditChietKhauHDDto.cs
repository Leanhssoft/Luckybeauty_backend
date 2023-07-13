using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto
{
    public class CreateOrEditChietKhauHDDto
    {
        public Guid Id { set; get; }
        public Guid IdChiNhanh { set; get; }
        public byte LoaiChietKhau { set; get; }
        public double? GiaTriChietKhau { set; get; } = 0;
        public List<string> ChungTuApDung { set; get; }
    }
}
