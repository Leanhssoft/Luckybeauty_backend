using System.Collections.Generic;

namespace BanHangBeautify.HangHoa.LoaiHangHoa.Dto
{
    public class LoaiHangHoaInfoDto
    {
        public int Id { set; get; }
        public string MaLoaiHangHoa { get; set; }
        public string TenLoaiHangHoa { get; set; }
        public List<string>? DichVus { set; get; }
    }
}
