using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.LoaiHangHoa.Dto
{
    public class LoaiHangHoaInfoDto
    {
        public Guid Id { set; get; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public List<string>? DichVus { set; get; }
    }
}
